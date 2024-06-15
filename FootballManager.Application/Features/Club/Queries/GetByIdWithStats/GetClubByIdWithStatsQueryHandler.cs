using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetByIdWithStats;

public record GetClubByIdWithStatsQuery(int ClubId) : IRequest<Result<GetClubByIdWithStatsResponse>>;

public class GetClubByIdWithStatsResponse
{
    public string ClubName { get; set; } = string.Empty;
    public string CoachName { get; set; } = string.Empty;
    public string StadiumName { get; set; } = string.Empty;
    public string ParticipatedChampionships { get; set; } = string.Empty;
    public double AverageSquadAge { get; set; }
    public int TotalPlayedMatches { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public int ScoredGoals { get; set; }
    public int ConcededGoals { get; set; }
}

public class GetClubByIdWithStatsQueryHandler(IClubRepository repository, IMapper mapper)
    : IRequestHandler<GetClubByIdWithStatsQuery, Result<GetClubByIdWithStatsResponse>>
{
    public async Task<Result<GetClubByIdWithStatsResponse>> Handle(GetClubByIdWithStatsQuery request, CancellationToken cancellationToken)
    {
        var club = await repository.GetAll()
                        .Include(c => c.Coach)
                        .Include(c => c.Players)
                        .Include(c => c.HomeMatches)
                            .ThenInclude(m => m.Goals)
                                .ThenInclude(g => g.Scorer)
                        .Include(c => c.AwayMatches)
                            .ThenInclude(m => m.Goals)
                                .ThenInclude(g => g.Scorer)
                        .Include(m => m.ParticipatingChampionships)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == request.ClubId, cancellationToken);

        if (club is null)
        {
            return new NotFoundResult<GetClubByIdWithStatsResponse>($"Club with ID {request.ClubId} not found");
        }

        return new SuccessResult<GetClubByIdWithStatsResponse>(mapper.Map<GetClubByIdWithStatsResponse>(club));
    }
}
