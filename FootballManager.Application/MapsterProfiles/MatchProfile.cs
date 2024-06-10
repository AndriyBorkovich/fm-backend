using FootballManager.Application.Features.Match.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Match.Queries.GetById;
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

        config.NewConfig<GoalAction, GoalEvent>()
            .Map(dest => dest.Minute, src => src.Minute)
            .Map(dest => dest.Scorer, src => src.Scorer.Name)
            .Map(dest => dest.Assist, src => src.Assistant != null ? src.Assistant.Name : string.Empty)
            .Map(dest => dest.IsHomeTeam, src => src.Scorer.ClubId == src.Match.HomeTeamId);

        config.NewConfig<Card, CardEvent>()
           .Map(dest => dest.Minute, src => src.Minute)
           .Map(dest => dest.PlayerName, src => src.Player.Name)
           .Map(dest => dest.CardType, src => Enum.GetName(typeof(CardType), src.Type))
           .Map(dest => dest.IsHomeTeam, src => src.Player.ClubId == src.Match.HomeTeamId);

        config.NewConfig<Match, GetMatchByIdResponse>()
            .Map(dest => dest.MatchDate, src => src.MatchDate)
            .Map(dest => dest.HomeTeamName, src => src.HomeTeam.Name)
            .Map(dest => dest.AwayTeamName, src => src.AwayTeam.Name)
            .Map(dest => dest.Result, src => GetMatchResultString(src.Result))
            .Map(dest => dest.Goals, src => src.Goals.Adapt<List<GoalEvent>>())
            .Map(dest => dest.Cards, src => src.Cards.Adapt<List<CardEvent>>())
            .AfterMapping((src, dest) =>
            {
                (dest.HomeTeamGoals, dest.AwayTeamGoals, _) = src.CalculateScore();
            });
    }

    private static string GetMatchResultString(MatchResult result)
    {
        return result switch
        {
            MatchResult.HomeTeamWin => "Home Team Win",
            MatchResult.AwayTeamWin => "Away Team Win",
            MatchResult.Draw => "Draw",
            _ => "Unknown"
        };
    }
}
