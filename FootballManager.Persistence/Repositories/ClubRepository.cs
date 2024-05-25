using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Persistence.Repositories;

public class ClubRepository : GenericRepository<Club>, IClubRepository
{
    public ClubRepository(FootballManagerContext context) : base(context)
    {
    }

    public IQueryable<Club> GetClubsWithPlayersInfo()
    {
        return GetAll()
            .Include(c => c.Players);
    }
    public async Task<Club?> GetClubWithMatchHistory(int clubId)
    {
        // TODO: rewrite to select
        /*return await GetAll()
                    .AsSplitQuery()
                    .Include(c => c.HomeMatches)
                        .ThenInclude(m => m.HomeTeam)
                    .Include(c => c.HomeMatches)
                        .ThenInclude(m => m.AwayTeam)
                    .Include(c => c.HomeMatches)
                        .ThenInclude(m => m.Goals)
                            .ThenInclude(g => g.Scorer)
                    .Include(c => c.AwayMatches)
                        .ThenInclude(m => m.HomeTeam)
                    .Include(c => c.AwayMatches)
                        .ThenInclude(m => m.AwayTeam)
                    .Include(c => c.AwayMatches)
                        .ThenInclude(m => m.Goals)
                            .ThenInclude(g => g.Scorer)
                    .FirstOrDefaultAsync(c => c.Id == clubId);*/
        return await GetAll()
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(c => c.HomeMatches)
                        .ThenInclude(m => m.AwayTeam)
                    .Include(c => c.HomeMatches)
                        .ThenInclude(m => m.Goals)
                            .ThenInclude(g => g.Scorer)
                    .Include(c => c.AwayMatches)
                        .ThenInclude(m => m.Goals)
                            .ThenInclude(g => g.Scorer)
                    .Include(c => c.AwayMatches)
                            .ThenInclude(m => m.HomeTeam)
                    .FirstOrDefaultAsync(c => c.Id == clubId);
    }

    public async Task<bool> PlayerExistsInClub(int playerId, int clubId)
    {
        return await GetAll()
                    .AsNoTracking()
                    .AnyAsync(c => c.Id == clubId && c.Players.Any(p => p.Id == playerId));
    }
}
