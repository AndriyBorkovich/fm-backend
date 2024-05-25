using System.Reflection;
using FluentValidation;
using FootballManager.Application.Features.Match.Commands.SimulateMatch;
using FootballManager.Application.Features.Player.Commands.CreatePlayer;
using FootballManager.Application.Features.Player.Commands.UpdatePlayer;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace FootballManager.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(mappingConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IValidator<CreatePlayerCommand>, CreatePlayerCommandValidator>();
        services.AddScoped<IValidator<UpdatePlayerCommand>, UpdatePlayerCommandValidator>();
        services.AddScoped<IValidator<SimulateMatchCommand>, SimulateMatchCommandValidator>();

        return services;
    }
}
