using System.Threading.RateLimiting;
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

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<FootballManagerContext>();
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

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    // test example  bombardier -c 1 -n 100 http://localhost:5285/api/Player/GetAll
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    rateLimiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext => RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(), factory: _ => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 20,
            QueueLimit = 0,
            Window = TimeSpan.FromMinutes(1)
        }));
});

// TODO: add double timespan
// var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
//     .WaitAndRetryAsync(3, retryAttempts => TimeSpan.FromSeconds(10));
//
// builder.Services.AddHttpClient<ISlackClient, SlackClient>().ConfigureHttpClient(
//     (serviceProvider, httpClient) =>
//     {
//         var httpClientOptions = serviceProvider.GetRequiredService<SlackClientOptions>();
//
//         httpClient.BaseAddress = httpClientOptions.BaseAddress;
//         httpClient.Timeout = httpClientOptions.Timeout;
//
//     }).AddPolicyHandler(retryPolicy);

var app = builder.Build();

app.UseExceptionHandler();

// using var container = app.Services.CreateScope();
// var dbContext = container.ServiceProvider.GetService<FootballManagerContext>();
// dbContext!.Database.Migrate();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.UseCors("All");

app.Run();
