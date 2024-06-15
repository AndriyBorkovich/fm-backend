using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;
using MatchEntity = FootballManager.Domain.Entities.Match;

namespace FootballManager.Application.Features.Season.Queries.GetTable;

public record GetSeasonTableQuery(int SeasonId) : IRequest<Result<List<GetSeasonTableResponse>>>;

public record GetSeasonTableResponse(
    string ClubName,
    int PlayedGames,
    int Wins,
    int Draws,
    int Losses,
    int GoalsFor,
    int GoalsAgainst,
    int GoalDifference,
    int Points);

public class GetSeasonTableQueryHandler(
    ISeasonRepository seasonRepository, IMatchRepository matchRepository) : IRequestHandler<GetSeasonTableQuery, Result<List<GetSeasonTableResponse>>>
{
    private class ClubStats
    {
        public string ClubName { get; set; } = string.Empty;
        public int PlayedGames { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Draws { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int GoalsFor { get; set; } = 0;
        public int GoalsAgainst { get; set; } = 0;
        public int GoalDifference => GoalsFor - GoalsAgainst;
        public int Points { get; set; } = 0;
    }

    public async Task<Result<List<GetSeasonTableResponse>>> Handle(GetSeasonTableQuery request, CancellationToken cancellationToken)
    {
        var seasonExists = await seasonRepository.AnyAsync(s => s.Id == request.SeasonId);
        if (!seasonExists)
        {
            return new InvalidResult<List<GetSeasonTableResponse>>($"Season with ID {request.SeasonId} does not exist");
        }

        var matches = await matchRepository.GetAll()
                                        .Include(m => m.Goals)
                                        .Include(m => m.Players)
                                        .Include(m => m.HomeTeam)
                                        .Include(m => m.AwayTeam)
                                        .Where(m => m.SeasonId == request.SeasonId)
                                        .AsNoTracking()
                                        .AsSplitQuery()
                                        .ToListAsync(cancellationToken);

        var table = ProcessResults(matches);

        var response = table.Values.Select(cs => new GetSeasonTableResponse(
            cs.ClubName,
            cs.PlayedGames,
            cs.Wins,
            cs.Draws,
            cs.Losses,
            cs.GoalsFor,
            cs.GoalsAgainst,
            cs.GoalDifference,
            cs.Points))
            .OrderByDescending(r => r.Points)
            .ThenByDescending(r => r.GoalDifference)
            .ThenByDescending(r => r.GoalsFor)
            .ToList();

        return new SuccessResult<List<GetSeasonTableResponse>>(response);
    }

    private static Dictionary<int, ClubStats> ProcessResults(List<MatchEntity> matches)
    {
        var table = new Dictionary<int, ClubStats>();
        foreach (var match in matches)
        {
            var (homeGoals, awayGoals, result) = match.CalculateScore();

            if (!table.ContainsKey(match.HomeTeamId))
                table[match.HomeTeamId] = new ClubStats { ClubName = match.HomeTeam.Name };

            if (!table.ContainsKey(match.AwayTeamId))
                table[match.AwayTeamId] = new ClubStats { ClubName = match.AwayTeam.Name };

            var homeStats = table[match.HomeTeamId];
            var awayStats = table[match.AwayTeamId];

            homeStats.PlayedGames++;
            awayStats.PlayedGames++;

            homeStats.GoalsFor += homeGoals;
            awayStats.GoalsFor += awayGoals;

            homeStats.GoalsAgainst += awayGoals;
            awayStats.GoalsAgainst += homeGoals;

            if (result == MatchResult.HomeTeamWin)
            {
                homeStats.Wins++;
                homeStats.Points += 3;
                awayStats.Losses++;
            }
            else if (result == MatchResult.AwayTeamWin)
            {
                awayStats.Wins++;
                awayStats.Points += 3;
                homeStats.Losses++;
            }
            else
            {
                homeStats.Draws++;
                awayStats.Draws++;
                homeStats.Points++;
                awayStats.Points++;
            }
        }

        return table;
    }
}
