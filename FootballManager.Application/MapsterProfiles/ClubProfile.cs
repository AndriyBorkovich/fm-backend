using FootballManager.Application.Features.Club.Queries.GetAllClubs;
using FootballManager.Application.Features.Club.Queries.GetClubWithMatchesHistory;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Mapster;

namespace FootballManager.Application.MapsterProfiles;

public class ClubProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Club, ClubShortInfoDTO>()
            .Map(dest => dest.Type, src => Enum.GetName(typeof(ClubType), src.Type));

        config.NewConfig<Club, ClubWithMatchHistoryDTO>()
            .Map(dest => dest.HomeMatchesResults, src => src.HomeMatches)
            .Map(dest => dest.AwayMatchesResults, src => src.AwayMatches);
    }
}
