using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Persistence.Repositories;

public class MatchRepository : GenericRepository<Match>, IMatchRepository
{
    public MatchRepository(FootballManagerContext context) : base(context)
    {
    }

    public async Task<List<Match>> GetMatchHistoryForClub(int clubId)
    {
        return await GetAll()
                        .AsNoTracking()
                        .Where(m => m.HomeTeamId == clubId || m.AwayTeamId == clubId)
                        .Select(m => new Match
                        {
                            Id = m.Id,
                            MatchDate = m.MatchDate,
                            Result = m.Result,
                            HomeTeamId = m.HomeTeamId,
                            HomeTeam = new Club { Name = m.HomeTeam.Name },
                            AwayTeamId = m.AwayTeamId,
                            AwayTeam = new Club { Name = m.AwayTeam.Name },
                            Goals = m.Goals.Select(g => new GoalAction
                            {
                                Id = g.Id,
                                Scorer = new Player
                                {
                                    ClubId = g.Scorer.ClubId,
                                }
                            }).ToList()
                        })
                        .OrderByDescending(m => m.MatchDate)
                        .ToListAsync();
    }
}
