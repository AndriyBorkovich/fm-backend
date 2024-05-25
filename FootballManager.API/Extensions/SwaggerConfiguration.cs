using System.Reflection;

namespace FootballManager.API.Extensions;

public static class SwaggerConfiguration
{
    /// <summary>
    /// configure Swagger
    /// </summary>
    /// <param name="services"></param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Football manager API",
                        Description = "An API for managing football matches, clubs, players",
                        Version = "v1"
                    });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            }
        );
    }
}
