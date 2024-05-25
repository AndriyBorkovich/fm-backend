using FluentValidation;
using FootballManager.Application.Contracts.Email;
using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using FootballManager.Application.Features.Shared.DTOs;
using FootballManager.Application.Models.Email;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Match.Commands.SimulateMatch;

public class SimulateMatchCommandHandler : IRequestHandler<SimulateMatchCommand, Result<MatchResultDTO>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IEmailSender _emailSender;
    private readonly IAppLogger<SimulateMatchCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<SimulateMatchCommand> _validator;

    public SimulateMatchCommandHandler(
        IClubRepository clubRepository,
        IMatchRepository matchRepository,
        IMapper mapper,
        IValidator<SimulateMatchCommand> validator,
        IEmailSender emailSender,
        IAppLogger<SimulateMatchCommandHandler> logger)
    {
        _clubRepository = clubRepository;
        _matchRepository = matchRepository;
        _mapper = mapper;
        _validator = validator;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<Result<MatchResultDTO>> Handle(SimulateMatchCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new InvalidResult<MatchResultDTO>(validationResult.Errors.ToResponse());
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

        await _matchRepository.InsertAsync(match);

        var result = _mapper.Map<MatchResultDTO>(match);

        await _matchRepository.UpdateAsync(match);

        // send email to the current logged user
        // TODO: change email
        var email = new EmailMessage()
        {
            To = "borkovich25andri@gmail.com",
            Body =
                $"Match {result.HomeTeamName} - {result.AwayTeamName} finished with the result {result.HomeTeamGoals}:{result.AwayTeamGoals}",
            Subject = "Match result"
        };

        var emailResult = await _emailSender.SendEmail(email);
        if (!emailResult)
        {
            _logger.LogWarning($"Failed to send email to {email.To}");
        }
        else
        {
            _logger.LogInformation("Email sent successfully");
        }

        return new SuccessResult<MatchResultDTO>(result);
    }

    private async Task<List<Domain.Entities.Player>> GetRandomPlayers(int clubId, int count)
    {
        var players = (await _clubRepository.GetClubsWithPlayersInfo()
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
