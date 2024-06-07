using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Persistence.DatabaseContext;

namespace FootballManager.Persistence.Repositories;

public class CoachRepository : GenericRepository<Coach>, ICoachRepository
{
    public CoachRepository(FootballManagerContext context) : base(context)
    {
    }
}
