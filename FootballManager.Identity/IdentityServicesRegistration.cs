using FootballManager.Application.Contracts.Identity;
using FootballManager.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FootballManager.Identity;

public static class IdentityServicesRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}
