using FootballManager.Application.Features.Club.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Club.Queries.GetByIdWithStats;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Mapster;

namespace FootballManager.Application.MapsterProfiles;

public class ClubProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Club, GetAllClubsShortInfoResponse>()
            .Map(dest => dest.PlayersCount, src => src.Players.Count)
            .Map(dest => dest.HeadCoachName, src => src.Coach.Name, srcCond => srcCond.Coach != null);

        config.NewConfig<Club, GetClubByIdWithStatsResponse>()
            .Map(dest => dest.ClubName, src => src.Name)
            .Map(dest => dest.CoachName, src => src.Coach != null ? src.Coach.Name : string.Empty)
            .Map(dest => dest.ParticipatedChampionships, src => string.Join(',', src.ParticipatingChampionships.Select(c => c.Name)))
            .Map(dest => dest.AverageSquadAge, src => Math.Round(src.Players.Average(p => p.CalculateAge()), 2))
            .Map(dest => dest.TotalPlayedMatches, src => src.HomeMatches.Count + src.AwayMatches.Count)
            .AfterMapping((src, dest) =>
            {
                var allMatches = src.HomeMatches.Concat(src.AwayMatches);

                dest.Wins = allMatches.Count(m =>
                    (m.Result == MatchResult.HomeTeamWin && m.HomeTeamId == src.Id) ||
                    (m.Result == MatchResult.AwayTeamWin && m.AwayTeamId == src.Id));

                dest.Losses = allMatches.Count(m =>
                    (m.Result == MatchResult.HomeTeamWin && m.AwayTeamId == src.Id) ||
                    (m.Result == MatchResult.AwayTeamWin && m.HomeTeamId == src.Id));

                dest.Draws = allMatches.Count(m => m.Result == MatchResult.Draw);

                dest.ConcededGoals = allMatches.Sum(m =>
                    m.Goals.Count(g => g.Scorer.ClubId != src.Id));

                dest.ScoredGoals = allMatches.Sum(m =>
                    m.Goals.Count(g => g.Scorer.ClubId == src.Id));
            }); ;
    }
}
