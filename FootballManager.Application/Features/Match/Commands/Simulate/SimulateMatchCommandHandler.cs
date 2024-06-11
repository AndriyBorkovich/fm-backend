using FluentValidation;
using FootballManager.Application.Contracts.Email;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Features.Shared.Responses;
using FootballManager.Application.Utilities;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;
using MatchEntity = FootballManager.Domain.Entities.Match;
using PlayerEntity = FootballManager.Domain.Entities.Player;
using SeasonEntity = FootballManager.Domain.Entities.Season;


namespace FootballManager.Application.Features.Match.Commands.Simulate;

public record SimulateMatchCommand
(
    int HomeTeamId,
    int AwayTeamId,
    int SeasonId
) : IRequest<Result<MatchResultResponse>>;

public class SimulateMatchCommandHandler(
    IClubRepository clubRepository,
    IMatchRepository matchRepository,
    ISeasonRepository seasonRepository,
    IMapper mapper,
    IValidator<SimulateMatchCommand> validator,
    IEmailSender emailSender) : IRequestHandler<SimulateMatchCommand, Result<MatchResultResponse>>
{
    public async Task<Result<MatchResultResponse>> Handle(SimulateMatchCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new InvalidResult<MatchResultResponse>(validationResult.ToString());
        }

        var homeTeamPlayers = await GetStartingSquad(request.HomeTeamId);
        var awayTeamPlayers = await GetStartingSquad(request.AwayTeamId);

        var currentSeason = await seasonRepository.GetByIdAsync(request.SeasonId);
        var matchDate = GetRandomDateInSeason(currentSeason);

        var cards = GenerateCards(homeTeamPlayers, awayTeamPlayers);
        var goals = GenerateGoals(homeTeamPlayers, awayTeamPlayers, cards);

        var allPlayers = homeTeamPlayers.Union(awayTeamPlayers).ToList();

        var match = new MatchEntity
        {
            HomeTeamId = request.HomeTeamId,
            AwayTeamId = request.AwayTeamId,
            MatchDate = matchDate,
            Goals = goals,
            Cards = cards,
            Players = allPlayers,
            SeasonId = request.SeasonId
        };

        await matchRepository.InsertAsync(match);

        // mapper sets match result so we need to update created entity
        var result = mapper.Map<MatchResultResponse>(match);

        await matchRepository.UpdateAsync(match);

        return new SuccessResult<MatchResultResponse>(result);
    }

    public static DateTime GetRandomDateInSeason(SeasonEntity season)
    {
        var rand = new Random();
        var start = new DateTime(season.StartYear, 8, 1);
        var end = new DateTime(season.EndYear, 5, 31);
        var range = (end - start).Days;
        return start.AddDays(rand.Next(range));
    }

    private async Task<List<PlayerEntity>> GetStartingSquad(int clubId)
    {
        var club = await clubRepository.GetClubsWithCoachAndPlayersInfo().FirstOrDefaultAsync(c => c.Id == clubId);
        var players = club!.Players;
        var coach = club!.Coach;

        var (defendersCount, midfieldersCount, forwardsCount) = HelperMethods.GetPositionsCount(coach!.PreferredFormation);

        var shuffledPlayers = players.OrderBy(x => Random.Shared.Next());
        var goalkeeper = shuffledPlayers.Where(p => p.Position == PlayerPosition.Goalkeeper).Take(1).ToList();
        var defenders = shuffledPlayers.Where(p => p.Position == PlayerPosition.Defender).Take(defendersCount).ToList();
        var midfielders = shuffledPlayers.Where(p => p.Position == PlayerPosition.Midfielder).Take(midfieldersCount).ToList();
        var forwards = shuffledPlayers.Where(p => p.Position == PlayerPosition.Forward).Take(forwardsCount).ToList();

        return [.. goalkeeper, .. defenders, .. midfielders, .. forwards];
    }

    // Method to generate goals
    public static List<GoalAction> GenerateGoals(List<PlayerEntity> homePlayers, List<PlayerEntity> awayPlayers, List<Card> cards)
    {
        const double AssistProbability = 0.6;
        var goals = new List<GoalAction>();

        var homeTeamScoringProbability = new Random().NextDouble();
        var activeHomePlayers = GetActivePlayers(homePlayers, cards);

        // Generate goals for the home team
        foreach (var player in activeHomePlayers.Where(p => p.Position != PlayerPosition.Goalkeeper))
        {
            // Simulate the probability of a player scoring a goal
            if (RandomEventOccurred(homeTeamScoringProbability))
            {
                var goal = new GoalAction
                {
                    CreatedDate = DateTime.UtcNow,
                    ScorerId = player.Id,
                    Scorer = player,
                    Minute = GenerateMinute(),
                    IsOwnGoal = false
                };

                // Decide if there's an assistant for the goal
                if (RandomEventOccurred(AssistProbability))
                {
                    var assistant = PickRandomPlayer(activeHomePlayers.Where(p => p.Id != player.Id));
                    goal.AssistantId = assistant.Id;
                }

                goals.Add(goal);
            }
        }

        var awayTeamScoringProbability = new Random().NextDouble();
        var activeAwayPlayers = GetActivePlayers(awayPlayers, cards);

        // Generate goals for the away team
        foreach (var player in activeAwayPlayers.Where(p => p.Position != PlayerPosition.Goalkeeper))
        {
            // Simulate the probability of a player scoring a goal
            if (RandomEventOccurred(awayTeamScoringProbability))
            {
                var goal = new GoalAction
                {
                    CreatedDate = DateTime.UtcNow,
                    ScorerId = player.Id,
                    Scorer = player,
                    Minute = GenerateMinute(),
                    IsOwnGoal = false
                };

                // Decide if there's an assistant for the goal
                if (RandomEventOccurred(AssistProbability))
                {
                    var assistant = PickRandomPlayer(activeAwayPlayers.Where(p => p.Id != player.Id));
                    goal.AssistantId = assistant.Id;
                }

                goals.Add(goal);
            }
        }

        return goals;

        // Method to exclude players who received red or 2 yellow cards during the match
        static List<PlayerEntity> GetActivePlayers(List<PlayerEntity> players, List<Card> cards)
        {
            var playerCardCounts = new Dictionary<int, int>();

            foreach (var card in cards)
            {
                if (!playerCardCounts.TryGetValue(card.PlayerId, out var value))
                {
                    value = 0;
                    playerCardCounts[card.PlayerId] = value;
                }

                if (card.Type == CardType.Yellow)
                {
                    playerCardCounts[card.PlayerId] = ++value;
                }
                else if (card.Type == CardType.Red)
                {
                    playerCardCounts[card.PlayerId] = 2; // Immediate removal
                }
            }

            return players.Where(player => !playerCardCounts.ContainsKey(player.Id) || playerCardCounts[player.Id] < 2).ToList();
        }

        static PlayerEntity PickRandomPlayer(IEnumerable<PlayerEntity> players)
        {
            return players.ElementAt(new Random().Next(players.Count()));
        }
    }

    public static List<Card> GenerateCards(List<PlayerEntity> homePlayers, List<PlayerEntity> awayPlayers)
    {
        var cards = new List<Card>();
        var playerCardCounts = new Dictionary<int, int>();

        // Generate cards for the home team
        foreach (var player in homePlayers)
        {
            if (PlayerCanReceiveCard(player.Id) && RandomEventOccurred(0.02))
            {
                var card = new Card
                {
                    CreatedDate = DateTime.UtcNow,
                    PlayerId = player.Id,
                    Minute = GenerateMinute(),
                    Type = GetRandomCardType()
                };

                cards.Add(card);
                UpdatePlayerCardCount(player.Id, card.Type);
            }
        }

        // Generate cards for the away team
        foreach (var player in awayPlayers)
        {
            if (PlayerCanReceiveCard(player.Id) && RandomEventOccurred(0.02))
            {
                var card = new Card
                {
                    PlayerId = player.Id,
                    Minute = GenerateMinute(),
                    Type = GetRandomCardType()
                };

                cards.Add(card);
                UpdatePlayerCardCount(player.Id, card.Type);
            }
        }

        return cards;

        // Method to select a random card type
        static CardType GetRandomCardType()
        {
            var values = Enum.GetValues(typeof(CardType));
            return (CardType)(values.GetValue(new Random().Next(values.Length)) ?? CardType.Yellow);
        }

        bool PlayerCanReceiveCard(int playerId)
        {
            if (!playerCardCounts.TryGetValue(playerId, out var value))
            {
                return true;
            }

            // Check if the player has already received a red card or two yellow cards
            return value < 2;
        }

        void UpdatePlayerCardCount(int playerId, CardType cardType)
        {
            if (!playerCardCounts.TryGetValue(playerId, out var value))
            {
                value = 0;
                playerCardCounts[playerId] = value;
            }

            if (cardType == CardType.Yellow)
            {
                playerCardCounts[playerId] = ++value;
            }
            else if (cardType == CardType.Red)
            {
                playerCardCounts[playerId] = 2; // Immediate removal
            }
        }
    }

    // Method to generate a random minute for a goal
    private static int GenerateMinute(bool isExtraTime = false)
    {
        var random = new Random();
        return !isExtraTime ? random.Next(1, 90) : random.Next(1, 121);
    }

    // Method to simulate a random event based on probability
    private static bool RandomEventOccurred(double probability)
    {
        return new Random().NextDouble() < probability;
    }
}
