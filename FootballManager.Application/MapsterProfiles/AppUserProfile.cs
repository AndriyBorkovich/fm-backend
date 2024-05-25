using FootballManager.Application.Models.Identity;
using FootballManager.Domain.Entities;
using Mapster;

namespace FootballManager.Application.MapsterProfiles;

public class AppUserProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegistrationRequest, AppUser>()
            .Map(dest => dest.UserName, src => $"{src.FirstName}{src.LastName}");
    }
}
