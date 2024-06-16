using FootballManager.Application.Contracts.Caching;
using FootballManager.Application.Contracts.Email;
using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Models.Email;
using FootballManager.Infrastructure.Cache;
using FootballManager.Infrastructure.EmailService;
using FootballManager.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FootballManager.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        var connectionString = configuration.GetConnectionString("Redis");

        var multiplexer = ConnectionMultiplexer.Connect(connectionString ?? string.Empty);

        if (multiplexer.IsConnected)
        {
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            services.AddStackExchangeRedisCache(redisOptions => {
                redisOptions.Configuration = connectionString;
            });

            services.AddSingleton<ICacheService, CacheService>();
        }
        else
        {
            // injecting service which simulates cache work (literraly it doesn't do anything)
            services.AddSingleton<ICacheService, FakeCacheService>();
        }

        return services;
    }
}
