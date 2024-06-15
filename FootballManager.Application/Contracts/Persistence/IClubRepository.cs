using FootballManager.Domain.Entities;

namespace FootballManager.Application.Contracts.Persistence;

public interface IClubRepository : IGenericRepository<Club>
{
    IQueryable<Club> GetAllShortInfo();
    IQueryable<Club> GetAllWithCoachAndPlayersInfo();
}
