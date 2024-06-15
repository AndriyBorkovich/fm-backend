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

    public IQueryable<Club> GetAllShortInfo()
    {
        return GetAll()
            .AsNoTracking()
            .Select(c => new Club
            {
                Id = c.Id,
                Name = c.Name,
                StadiumName = c.StadiumName,
                Type = c.Type,
                Coach = c.Coach != null ? new Coach { Name = c.Coach.Name } : null,
                Players = c.Players.Select(p => new Player
                {
                    Id = p.Id
                }).ToList()
            });
    }

    public IQueryable<Club> GetAllWithCoachAndPlayersInfo()
    {
        return GetAll()
            .Include(c => c.Coach)
            .Include(c => c.Players);
    }
}
