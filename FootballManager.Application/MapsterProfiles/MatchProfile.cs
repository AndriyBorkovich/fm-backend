using FootballManager.Application.Features.Shared.DTOs;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Mapster;

namespace FootballManager.Application.MapsterProfiles;

public class MatchProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Match, MatchResultDTO>()
            .Map(dest => dest.MatchId, src => src.Id)
            .Map(dest => dest.Date, src => src.MatchDate.ToString("dd.MM.yy h:mm"))
            .Map(dest => dest.HomeTeamName, src => src.HomeTeam.Name)
            .Map(dest => dest.AwayTeamName, src => src.AwayTeam.Name)
            .AfterMapping((src, dest) =>
            {
                var score = src.CalculateScore();
                dest.HomeTeamGoals = score.homeGoals;
                dest.AwayTeamGoals = score.awayGoals;
                dest.Result = Enum.GetName(typeof(MatchResult), score.result) ?? string.Empty;
            });
    }
}
