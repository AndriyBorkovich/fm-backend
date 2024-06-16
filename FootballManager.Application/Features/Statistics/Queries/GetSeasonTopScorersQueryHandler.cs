using FootballManager.Application.Contracts.Caching;
using FootballManager.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Statistics.Queries
{
    public record GetSeasonTopScorersQuery(int SeasonId) : IRequest<Result<List<GetTopScorersResponse>>>;

    public record GetTopScorersResponse(
        int PlayerId,
        string ClubName,
        string Name,
        int GoalsCount);

    public class GetSeasonTopScorersQueryHandler(
        ISeasonRepository seasonRepository,
        IPlayerRepository playerRepository,
        ICacheService cache) : IRequestHandler<GetSeasonTopScorersQuery, Result<List<GetTopScorersResponse>>>
    {
        public async Task<Result<List<GetTopScorersResponse>>> Handle(GetSeasonTopScorersQuery request, CancellationToken cancellationToken)
        {
            var seasonExists = await seasonRepository.AnyAsync(s => s.Id == request.SeasonId);
            if (!seasonExists)
            {
                return new NotFoundResult<List<GetTopScorersResponse>>($"Season with ID {request.SeasonId} not found");
            }

            return new SuccessResult<List<GetTopScorersResponse>>(await TryGetFromCacheAsync(request, cancellationToken));
        }

        private async Task<List<GetTopScorersResponse>> TryGetFromCacheAsync(GetSeasonTopScorersQuery request, CancellationToken cancellationToken)
        {
            var key = $"TopScorers_{request.SeasonId}";

            var cachedPlayers = await cache.GetRecordAsync<List<GetTopScorersResponse>>(key, cancellationToken);

            if (cachedPlayers is not null)
            {
                return cachedPlayers;
            }

            var players = await playerRepository.GetAll()
                                        .Where(p => p.Matches.Any(m => m.SeasonId == request.SeasonId))
                                        .Include(p => p.CurrentClub)
                                        .Include(p => p.ScoredGoals.Where(g => !g.IsOwnGoal && g.Match.SeasonId == request.SeasonId))
                                        .AsNoTracking()
                                        .ToListAsync(cancellationToken);

            var topScorers = players
                    .Select(p => new GetTopScorersResponse(
                        p.Id,
                        p.CurrentClub?.Name ?? string.Empty,
                        p.Name,
                        p.ScoredGoals.Count
                    ))
                    .OrderByDescending(r => r.GoalsCount)
                    .Take(10)
                    .ToList();

            await cache.SetRecordAsync(
               key,
               topScorers,
               absoluteExpireTime: TimeSpan.FromMinutes(10),
               cancellationToken: cancellationToken);

            return topScorers;
        }
    }
}
