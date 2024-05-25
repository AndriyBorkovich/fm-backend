using FootballManager.Persistence.DatabaseContext;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FootballManager.API.HealthChecks;

/// <inheritdoc />
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly FootballManagerContext _context;

    public DatabaseHealthCheck(FootballManagerContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            var result = await _context.Database.CanConnectAsync(cancellationToken);

            var healthCheckResult = result ? HealthCheckResult.Healthy("DB is running!") :
                HealthCheckResult.Unhealthy("Database is down, check if it is running");

            return healthCheckResult;
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("An error occurred while checking the database health", ex);
        }
    }
}
