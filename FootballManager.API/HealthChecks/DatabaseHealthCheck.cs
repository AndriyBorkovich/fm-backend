using FootballManager.Persistence.DatabaseContext;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FootballManager.API.HealthChecks;

/// <inheritdoc />
public class DatabaseHealthCheck(FootballManagerContext context, ILogger<DatabaseHealthCheck> logger) : IHealthCheck
{
    private readonly FootballManagerContext _context = context;

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            var result = await _context.Database.CanConnectAsync(cancellationToken);

            var healthCheckResult = result ? HealthCheckResult.Healthy("DB is running!") :
                HealthCheckResult.Unhealthy("Database is down, check if it is running");

            logger.LogInformation("DB Health check: {0}", healthCheckResult.Description);

            return healthCheckResult;
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("An error occurred while checking the database health", ex);
        }
    }
}
