using FootballManager.Domain.Entities;

namespace FootballManager.Application.Contracts.Persistence;

public interface IClubRepository : IGenericRepository<Club>
{
    IQueryable<Club> GetClubsWithCoachAndPlayersInfo();
    Task<bool> PlayerExistsInClub(int playerId, int clubId);
}
