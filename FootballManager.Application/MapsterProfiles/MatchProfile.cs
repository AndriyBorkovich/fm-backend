using FootballManager.Application.Features.Match.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Shared.Responses;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Mapster;

namespace FootballManager.Application.MapsterProfiles;

public class MatchProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Match, MatchResultResponse>()
            .Map(dest => dest.MatchId, src => src.Id)
            .Map(dest => dest.Date, src => src.MatchDate.ToString("dd.MM.yy h:mm"))
            .Map(dest => dest.HomeTeamName, src => src.HomeTeam.Name)
            .Map(dest => dest.AwayTeamName, src => src.AwayTeam.Name)
            .AfterMapping((src, dest) =>
            {
                var (homeGoals, awayGoals, result) = src.CalculateScore();
                dest.HomeTeamGoals = homeGoals;
                dest.AwayTeamGoals = awayGoals;
                dest.Result = Enum.GetName(typeof(MatchResult), result) ?? string.Empty;
            });

        config.NewConfig<Match, GetAllMatchesShortInfoResponse>()
            .Map(dest => dest.Match, src => src)
            .AfterMapping((src, dest) =>
            {
                var diff = (src.MatchDate - DateTime.UtcNow).TotalMinutes;
                dest.IsLive = diff >= 0 && diff <= 5;
            });
    }
}
