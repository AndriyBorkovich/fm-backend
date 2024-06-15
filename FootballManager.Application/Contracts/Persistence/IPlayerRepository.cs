using FootballManager.Domain.Entities;

namespace FootballManager.Application.Contracts.Persistence;

public interface IPlayerRepository : IGenericRepository<Player>
{
    Task<List<Player>> GetPlayersShortInfo();
    Task<Player?> GetByIdWithStatsAsync(int playerId);
    IQueryable<Player> GetPlayersWithoutClub();
}
