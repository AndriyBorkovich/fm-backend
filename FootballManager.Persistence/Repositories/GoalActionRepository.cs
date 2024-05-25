using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Persistence.DatabaseContext;

namespace FootballManager.Persistence.Repositories;

public class GoalActionRepository : GenericRepository<GoalAction>, IGoalActionRepository
{
    public GoalActionRepository(FootballManagerContext context) : base(context)
    {
    }
}