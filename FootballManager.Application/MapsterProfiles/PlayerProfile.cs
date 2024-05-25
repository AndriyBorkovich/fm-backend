using FootballManager.Application.Extensions;
using FootballManager.Application.Features.Player.Queries.GetAllPlayers;
using FootballManager.Application.Features.Player.Queries.GetPlayerWithStats;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Mapster;

namespace FootballManager.Application.MapsterProfiles;

public class PlayerProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Player, PlayerShortInfoDTO>()
            .Map(dest => dest.PlayerId, src => src.Id)
            .Map(dest => dest.Age, src => src.CalculateAge())
            .Map(dest => dest.Position, src => Enum.GetName(typeof(PlayerPosition), src.Position))
            .TwoWays();

        config.NewConfig<Player, PlayerStatisticDTO>()
            .Map(dest => dest.ClubName, src => src.CurrentClub.Name, srcCond => srcCond.CurrentClub != null)
            .Map(dest => dest.Age, src => src.CalculateAge())
            .Map(dest => dest.MatchesPlayed, src => src.Matches.Count)
            .Map(dest => dest.GoalsScored, src => src.ScoredGoals.Count(g => !g.IsOwnGoal))
            .Map(dest => dest.Assists, src => src.AssistedGoals.Count)
            .Map(dest => dest.RedCards, src => src.Cards.Count(c =>c.Type == CardType.Red))
            .Map(dest => dest.YellowCards, src => src.Cards.Count(c =>c.Type == CardType.Yellow))
            .Map(dest => dest.Position, src => Enum.GetName(typeof(PlayerPosition), src.Position));
    }
}
