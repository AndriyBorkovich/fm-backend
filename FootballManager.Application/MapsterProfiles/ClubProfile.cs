using FootballManager.Application.Features.Club.Queries.GetAllShortInfo;
using FootballManager.Domain.Entities;
using Mapster;

namespace FootballManager.Application.MapsterProfiles;

public class ClubProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Club, GetAllClubsShortInfoResponse>()
            .Map(dest => dest.PlayersCount, src => src.Players.Count)
            .Map(dest => dest.HeadCoachName, src => src.Coach.Name, srcCond => srcCond.Coach != null);
    }
}
