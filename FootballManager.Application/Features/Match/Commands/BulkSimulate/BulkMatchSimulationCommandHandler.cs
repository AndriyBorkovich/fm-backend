using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Features.Match.Commands.Simulate;
using FootballManager.Application.Utilities;
using FootballManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;
using MatchEntity = FootballManager.Domain.Entities.Match;
using PlayerEntity = FootballManager.Domain.Entities.Player;

namespace FootballManager.Application.Features.Match.Commands.BulkSimulate;

public record BulkMatchSimulationCommand(List<SimulateMatchCommand> MatchPairs) : IRequest<Result<Unit>>;
public class BulkMatchSimulationCommandHandler(
    IClubRepository clubRepository,
    ISeasonRepository seasonRepository,
    IMatchRepository matchRepository,
    IPlayerRepository playerRepository,
    IAppLogger<BulkMatchSimulationCommandHandler> logger
    ) : IRequestHandler<BulkMatchSimulationCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(BulkMatchSimulationCommand request, CancellationToken cancellationToken)
    {
        var list = new List<MatchEntity>();
        try
        {
            foreach (var pair in request.MatchPairs)
            {
                var homeTeamPlayers = await GetStartingSquad(pair.HomeTeamId);
                var awayTeamPlayers = await GetStartingSquad(pair.AwayTeamId);

                var currentSeason = await seasonRepository.GetByIdAsync(pair.SeasonId);
                var matchDate = SimulateMatchCommandHandler.GetRandomDateInSeason(currentSeason);

                var cards = SimulateMatchCommandHandler.GenerateCards(homeTeamPlayers, awayTeamPlayers);
                var goals = SimulateMatchCommandHandler.GenerateGoals(homeTeamPlayers, awayTeamPlayers, cards);

                var allPlayers = homeTeamPlayers.Union(awayTeamPlayers).ToList();

                var match = new MatchEntity
                {
                    CreatedDate = DateTime.UtcNow,
                    HomeTeamId = pair.HomeTeamId,
                    AwayTeamId = pair.AwayTeamId,
                    MatchDate = matchDate,
                    Goals = goals,
                    Cards = cards,
                    Players = allPlayers,
                    SeasonId = pair.SeasonId
                };

                list.Add(match);
            }

            await matchRepository.BulkInsertAsync(list);
            await matchRepository.CommitAsync();
            foreach (var m in list)
            {
                m.CalculateScore();
                m.UpdatedDate = DateTime.UtcNow;
                foreach (var p in m.Players)
                {
                    matchRepository.RegisterPlayerInMatch(m.Id, p.Id);
                }
            }

            await matchRepository.BulkUpdateAsync(list);

            logger.LogInformation("Bulk simulation for {0} matches was completed successfully", list.Count);

            return new SuccessResult<Unit>(Unit.Value);
        }
        catch (Exception ex)
        {
            logger.LogError("Bulk simulation for {0} matches failed: {1}", list.Count, ex.Message);
            return new InvalidResult<Unit>($"Bulk insert failed: {ex.Message}");
        }

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
}
