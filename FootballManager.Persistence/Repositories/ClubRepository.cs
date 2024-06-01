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

    public async Task<bool> PlayerExistsInClub(int playerId, int clubId)
    {
        return await GetAll()
                    .AsNoTracking()
                    .AnyAsync(c => c.Id == clubId && c.Players.Any(p => p.Id == playerId));
    }
}
