using FluentValidation;
using FootballManager.Application.Contracts.Email;
using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Features.Shared.Responses;
using FootballManager.Application.Models.Email;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;
using MatchEntity = FootballManager.Domain.Entities.Match;
using PlayerEntity = FootballManager.Domain.Entities.Player;

namespace FootballManager.Application.Features.Match.Commands.Simulate;

public record SimulateMatchCommand(int HomeTeamId, int AwayTeamId) : IRequest<Result<MatchResultResponse>>;

public class SimulateMatchCommandHandler(
    IClubRepository clubRepository,
    IMatchRepository matchRepository,
    IMapper mapper,
    IValidator<SimulateMatchCommand> validator,
    IEmailSender emailSender,
    IAppLogger<SimulateMatchCommandHandler> logger) : IRequestHandler<SimulateMatchCommand, Result<MatchResultResponse>>
{
    public async Task<Result<MatchResultResponse>> Handle(SimulateMatchCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new InvalidResult<MatchResultResponse>(validationResult.ToString());
        }

        var homeTeamPlayers = await GetRandomPlayers(request.HomeTeamId);
        var awayTeamPlayers = await GetRandomPlayers(request.AwayTeamId);

        var matchDate = DateTime.Now.AddDays(new Random().Next(-100, 100));

        var goals = GenerateGoals(homeTeamPlayers, awayTeamPlayers);
        var cards = GenerateCards(homeTeamPlayers, awayTeamPlayers);

        var allPlayers = homeTeamPlayers.Union(awayTeamPlayers).ToList();

        var match = new MatchEntity
        {
            HomeTeamId = request.HomeTeamId,
            AwayTeamId = request.AwayTeamId,
            MatchDate = matchDate,
            Goals = goals,
            Cards = cards,
            Players = allPlayers
        };

        await matchRepository.InsertAsync(match);

        var result = mapper.Map<MatchResultResponse>(match);

        await matchRepository.UpdateAsync(match);

        // TODO: Change library for sending emails
        //// send email to the current logged user
        //var email = new EmailMessage()
        //{
        //    To = "borkovich25andri@gmail.com",
        //    Body =
        //        $"Match {result.HomeTeamName} - {result.AwayTeamName} finished with the result {result.HomeTeamGoals}:{result.AwayTeamGoals}",
        //    Subject = "Match result"
        //};

        //var emailResult = await emailSender.SendEmail(email);
        //if (!emailResult)
        //{
        //    logger.LogWarning($"Failed to send email to {email.To}");
        //}
        //else
        //{
        //    logger.LogInformation("Email sent successfully");
        //}

        return new SuccessResult<MatchResultResponse>(result);
    }

    private async Task<List<PlayerEntity>> GetRandomPlayers(int clubId, int count = 11)
    {
        var players = (await clubRepository.GetClubsWithPlayersInfo()
                                        .FirstOrDefaultAsync(c => c.Id == clubId))?.Players;

        // Shuffle the players and select the first 'count' players
        players = players?.OrderBy(x => Random.Shared.Next()).ToList();

        return players?.Take(count).ToList() ?? [];
    }

    // Method to generate goals
    private static List<GoalAction> GenerateGoals(List<PlayerEntity> homePlayers, List<PlayerEntity> awayPlayers)
    {
        var goals = new List<GoalAction>();
        var homeProbability = new Random().NextDouble();
        var awayProbability = new Random().NextDouble();

        // Generate goals for the home team
        foreach (var player in homePlayers)
        {
            // Simulate the probability of a player scoring a goal
            if (RandomEventOccurred(homeProbability))
            {
                var goal = new GoalAction
                {
                    ScorerId = player.Id,
                    Minute = GenerateMinute(),
                    IsOwnGoal = false
                };

                // Decide if there's an assistant for the goal
                if (RandomEventOccurred(0.3))
                {
                    var assistant = SelectRandomPlayer(homePlayers.Where(p => p.Id != player.Id));
                    goal.AssistantId = assistant.Id;
                }

                goals.Add(goal);
            }
        }

        // Generate goals for the away team
        foreach (var player in awayPlayers)
        {
            // Simulate the probability of a player scoring a goal
            if (RandomEventOccurred(awayProbability))
            {
                var goal = new GoalAction
                {
                    ScorerId = player.Id,
                    Minute = GenerateMinute(),
                    IsOwnGoal = false
                };

                // Decide if there's an assistant for the goal
                if (RandomEventOccurred(0.3))
                {
                    var assistant = SelectRandomPlayer(awayPlayers.Where(p => p.Id != player.Id));
                    goal.AssistantId = assistant.Id;
                }

                goals.Add(goal);
            }
        }

        return goals;
    }

    private static List<Card> GenerateCards(List<PlayerEntity> homePlayers, List<PlayerEntity> awayPlayers)
    {
        var cards = new List<Card>();

        // Generate cards for the home team
        foreach (var player in homePlayers)
        {
            // Simulate the probability of a player receiving a card
            if (RandomEventOccurred(0.02)) // Adjust the probability as needed
            {
                var card = new Card
                {
                    PlayerId = player.Id,
                    Minute = GenerateMinute(),
                    Type = GetRandomCardType()
                };

                cards.Add(card);
            }
        }

        // Generate cards for the away team
        foreach (var player in awayPlayers)
        {
            // Simulate the probability of a player receiving a card
            if (RandomEventOccurred(0.02)) // Adjust the probability as needed
            {
                var card = new Card
                {
                    PlayerId = player.Id,
                    Minute = GenerateMinute(),
                    Type = GetRandomCardType()
                };

                cards.Add(card);
            }
        }

        return cards;
    }

    // Method to generate a random minute for a goal
    private static int GenerateMinute()
    {
        return new Random().Next(1, 121);
    }

    // Method to select a random player from the given list
    private static PlayerEntity SelectRandomPlayer(IEnumerable<PlayerEntity> players)
    {
        return players.ElementAt(new Random().Next(players.Count()));
    }

    // Method to select a random card type
    private static CardType GetRandomCardType()
    {
        var values = Enum.GetValues(typeof(CardType));
        return (CardType)(values.GetValue(new Random().Next(values.Length)) ?? CardType.Yellow);
    }

    // Method to simulate a random event based on probability
    private static bool RandomEventOccurred(double probability)
    {
        return new Random().NextDouble() < probability;
    }
}
