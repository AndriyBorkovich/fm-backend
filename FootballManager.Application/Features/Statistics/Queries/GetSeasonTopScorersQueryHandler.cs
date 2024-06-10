using FootballManager.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Statistics.Queries
{
    public record GetSeasonTopScorersQuery(int SeasonId) : IRequest<Result<List<GetSeasonTopScorersResponse>>>;

    public record GetSeasonTopScorersResponse(
        int PlayerId,
        string Name,
        int GoalsCount);

    public class GetSeasonTopScorersQueryHandler(
        ISeasonRepository seasonRepository,
        IPlayerRepository playerRepository) : IRequestHandler<GetSeasonTopScorersQuery, Result<List<GetSeasonTopScorersResponse>>>
    {
        public async Task<Result<List<GetSeasonTopScorersResponse>>> Handle(GetSeasonTopScorersQuery request, CancellationToken cancellationToken)
        {
            var seasonExists = await seasonRepository.AnyAsync(s => s.Id == request.SeasonId);
            if (!seasonExists)
            {
                return new NotFoundResult<List<GetSeasonTopScorersResponse>>($"Season with ID {request.SeasonId} not found");
            }

            var players = await playerRepository.GetAll()
                                        .Where(p => p.Matches.Any(m => m.SeasonId == request.SeasonId))
                                        .Include(p => p.ScoredGoals.Where(g => !g.IsOwnGoal && g.Match.SeasonId == request.SeasonId))
                                        .AsNoTracking()
                                        .ToListAsync(cancellationToken);

            var topScorers = players
                    .Select(p => new GetSeasonTopScorersResponse(
                        p.Id,
                        p.Name,
                        p.ScoredGoals.Count
                    ))
                    .OrderByDescending(r => r.GoalsCount)
                    .Take(10)
                    .ToList();

            return new SuccessResult<List<GetSeasonTopScorersResponse>>(topScorers);
        }
    }
}
