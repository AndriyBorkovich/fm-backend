using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Persistence.DatabaseContext;

namespace FootballManager.Persistence.Repositories;

public class SeasonRepository : GenericRepository<Season>, ISeasonRepository
{
    public SeasonRepository(FootballManagerContext context) : base(context)
    {
    }
}
