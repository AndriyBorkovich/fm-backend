using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FootballManager.API.Extensions;

/// <summary>
/// Extensions for services configuration
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure Swagger
    /// </summary>
    /// <param name="services"></param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Football manager API",
                    Description = "An API for managing football matches, clubs, players",
                    Version = "v1"
                });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        }
        );
    }

    /// <summary>
    /// Add jwt authorization based on configuration
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(jwtSettings.GetSection("securityKey").Value))
            };
        });
    }

    /// <summary>
    /// Add Polly retry policy for some third-party services
    /// </summary>
    /// <param name="services"></param>
    public static void AddRetryPolicy(this IServiceCollection services)
    {
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
    }

    /// <summary>
    /// Add rate limit for incoming requests to the API
    /// </summary>
    /// <param name="services"></param>
    public static void AddRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            // test example  bombardier -c 1 -n 100 http://localhost:5285/api/Player/GetAll
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                httpContext => RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(), factory: _ => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 50,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(2)
                }));
        });
    }
}
