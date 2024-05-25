using FootballManager.Application.MapsterProfiles;
using Mapster;
using MapsterMapper;

namespace FootballManager.Application.UnitTests;

public class MapsterConfiguration
{
    public static Mapper GetMapper()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(PlayerProfile).Assembly);
        return new Mapper(config);
    }
}
