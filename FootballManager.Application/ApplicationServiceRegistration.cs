using System.Reflection;
using FluentValidation;
using FootballManager.Application.Features.Club.Commands.Create;
using FootballManager.Application.Features.Coach.Commands.Create;
using FootballManager.Application.Features.Match.Commands.Schedule;
using FootballManager.Application.Features.Match.Commands.Simulate;
using FootballManager.Application.Features.Player.Commands.Create;
using FootballManager.Application.Features.Player.Commands.Update;
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

        AddValidators(services);

        return services;
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateClubCommand>, CreateClubCommandValidator>();
        services.AddScoped<IValidator<CreatePlayerCommand>, CreatePlayerCommandValidator>();
        services.AddScoped<IValidator<UpdatePlayerCommand>, UpdatePlayerCommandValidator>();
        services.AddScoped<IValidator<SimulateMatchCommand>, SimulateMatchCommandValidator>();
        services.AddScoped<IValidator<CreateCoachCommand>, CreateCoachCommandValidator>();
        services.AddScoped<IValidator<ScheduleMatchCommand>, ScheduleMatchCommandValidator>();
    }
}
