using FootballManager.API.Extensions;
using FootballManager.API.HealthChecks;
using FootballManager.Application;
using FootballManager.Infrastructure;
using FootballManager.Persistence;
using FootballManager.API.Middlewares;
using FootballManager.Domain.Entities;
using FootballManager.Identity;
using FootballManager.Persistence.DatabaseContext;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// add controllers and Swagger documentation
builder.Services.AddControllers()
    .AddJsonOptions(
        options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// exceptions handling
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
        ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
    }
);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// add services from other layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 7;
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<FootballManagerContext>();
builder.Services.AddIdentityServices();

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddPersistenceServices(builder.Configuration);

// configure API infrastructure
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("DB");

builder.Services.AddCors(options =>
{
    options.AddPolicy("All",
        policyConfig => policyConfig.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddRateLimiter();

builder.Services.AddRetryPolicy();

var app = builder.Build();

app.UseExceptionHandler();

app.Services.MigrateDatabase();
app.Services.SeedDatabase();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("All");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();
