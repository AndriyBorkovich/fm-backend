using FootballManager.Application.Contracts.Persistence;
using FootballManager.Persistence.DatabaseContext;
using FootballManager.Persistence.Repositories;
using FootballManager.Persistence.Seed;
using FootballManager.Persistence.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FootballManager.Persistence;

public static class PersistenceServiceRegistrations
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FootballManagerContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Default"));
            opt.UseTriggers(triggerConfig =>
            {
                triggerConfig.AddTrigger<BaseEntityUpdatesTrigger>();
            });
        });

        AddRepositories(services);

        services.AddScoped<IEntitiesSeeder, EnitiesSeeder>();

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IClubRepository, ClubRepository>();
        services.AddScoped<IGoalActionRepository, GoalActionRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<ICoachRepository, CoachRepository>();
        services.AddScoped<ISeasonRepository, SeasonRepository>();
    }
}
