using FluentValidation;
using FootballManager.Application.Contracts.Email;
using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using FootballManager.Application.Features.Shared.Responses;
using FootballManager.Application.Models.Email;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

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
            return new InvalidResult<MatchResultResponse>(validationResult.Errors.ToResponse());
        }

        var homeTeamPlayers = await GetRandomPlayers(request.HomeTeamId, 11);
        var awayTeamPlayers = await GetRandomPlayers(request.AwayTeamId,11);

        var matchDate = DateTime.Now.AddDays(new Random().Next(-100, 100));

        var goals = GenerateGoals(homeTeamPlayers, awayTeamPlayers);
        var cards = GenerateCards(homeTeamPlayers, awayTeamPlayers);

        var allPlayers = homeTeamPlayers.Union(awayTeamPlayers).ToList();

        var match = new Domain.Entities.Match
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

        // send email to the current logged user
        // TODO: change email
        var email = new EmailMessage()
        {
            To = "borkovich25andri@gmail.com",
            Body =
                $"Match {result.HomeTeamName} - {result.AwayTeamName} finished with the result {result.HomeTeamGoals}:{result.AwayTeamGoals}",
            Subject = "Match result"
        };

        var emailResult = await emailSender.SendEmail(email);
        if (!emailResult)
        {
            logger.LogWarning($"Failed to send email to {email.To}");
        }
        else
        {
            logger.LogInformation("Email sent successfully");
        }

        return new SuccessResult<MatchResultResponse>(result);
    }

    private async Task<List<Domain.Entities.Player>> GetRandomPlayers(int clubId, int count)
    {
        var players = (await clubRepository.GetClubsWithPlayersInfo()
                                        .FirstOrDefaultAsync(c => c.Id == clubId))?.Players;

        // Shuffle the players and select the first 'count' players
        players = players?.OrderBy(x => Random.Shared.Next()).ToList();

        return players?.Take(count).ToList() ?? [];
    }

    // Method to generate goals
    private List<GoalAction> GenerateGoals(List<Domain.Entities.Player> homePlayers, List<Domain.Entities.Player> awayPlayers)
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
                    var assistant = SelectRandomPlayer(awayPlayers);
                    goal.AssistantId = assistant.Id;
                }

                goals.Add(goal);
            }
        }

        return goals;
    }

    private List<Card> GenerateCards(List<Domain.Entities.Player> homePlayers, List<Domain.Entities.Player> awayPlayers)
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
    private int GenerateMinute()
    {
        return new Random().Next(1, 121);
    }

    // Method to select a random player from the given list
    private Domain.Entities.Player SelectRandomPlayer(IEnumerable<Domain.Entities.Player> players)
    {
        return players.ElementAt(new Random().Next(players.Count()));
    }

    // Method to select a random card type
    private CardType GetRandomCardType()
    {
        var values = Enum.GetValues(typeof(CardType));
        return (CardType)(values.GetValue(new Random().Next(values.Length)) ?? CardType.Yellow);
    }

    // Method to simulate a random event based on probability
    private bool RandomEventOccurred(double probability)
    {
        return new Random().NextDouble() < probability;
    }
}
