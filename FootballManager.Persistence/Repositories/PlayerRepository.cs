using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Persistence.Repositories;

public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
{
    public PlayerRepository(FootballManagerContext context) : base(context)
    {
    }

    public Task<List<Player>> GetPlayersShortInfo()
    {
        return GetAll().AsNoTracking().ToListAsync();
    }

    public Task<Player?> GetByIdWithStatsAsync(int playerId)
    {
        return GetAll()
            .AsNoTracking()
            .Include(p => p.CurrentClub)
            .Include(p => p.ScoredGoals)
            .Include(p => p.AssistedGoals)
            .Include(p => p.Matches)
            .Include(p => p.Cards)
            .FirstOrDefaultAsync(p => p.Id == playerId);
    }

    public IQueryable<Player> GetPlayersWithoutClub()
    {
        return GetAll().Where(p => p.ClubId == null);
    }
}
