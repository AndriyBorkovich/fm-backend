using FootballManager.Application.Features.Club.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Club.Queries.GetWithMatchesHistory;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Mapster;

namespace FootballManager.Application.MapsterProfiles;

public class ClubProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Club, GetAllClubShortInfoResponse>()
            .Map(dest => dest.Type, src => Enum.GetName(typeof(ClubType), src.Type));

        config.NewConfig<Club, GetClubWithMatchHistoryResponse>()
            .Map(dest => dest.HomeMatchesResults, src => src.HomeMatches)
            .Map(dest => dest.AwayMatchesResults, src => src.AwayMatches);
    }
}
