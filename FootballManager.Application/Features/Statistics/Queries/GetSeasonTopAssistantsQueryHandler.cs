using FootballManager.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Statistics.Queries;

public record GetSeasonTopAssistantsQuery(int SeasonId) : IRequest<Result<List<GetTopAssistantsResponse>>>;
public record GetTopAssistantsResponse(
    int PlayerId,
    string ClubName,
    string Name,
    int AssistsCount);
public class GetSeasonTopAssistantsQueryHandler(
     ISeasonRepository seasonRepository,
     IPlayerRepository playerRepository)
        : IRequestHandler<GetSeasonTopAssistantsQuery, Result<List<GetTopAssistantsResponse>>>
{
    public async Task<Result<List<GetTopAssistantsResponse>>> Handle(GetSeasonTopAssistantsQuery request, CancellationToken cancellationToken)
    {
        var seasonExists = await seasonRepository.AnyAsync(s => s.Id == request.SeasonId);
        if (!seasonExists)
        {
            return new NotFoundResult<List<GetTopAssistantsResponse>>($"Season with ID {request.SeasonId} not found");
        }

        var players = await playerRepository.GetAll()
                                    .Where(p => p.Matches.Any(m => m.SeasonId == request.SeasonId))
                                    .Include(p => p.CurrentClub)
                                    .Include(p => p.AssistedGoals.Where(g => !g.IsOwnGoal && g.Match.SeasonId == request.SeasonId))
                                    .AsNoTracking()
                                    .ToListAsync(cancellationToken);

        var topAssistants = players
                .Select(p => new GetTopAssistantsResponse(
                    p.Id,
                    p.CurrentClub?.Name ?? string.Empty,
                    p.Name,
                    p.AssistedGoals.Count
                ))
                .OrderByDescending(r => r.AssistsCount)
                .Take(10)
                .ToList();

        return new SuccessResult<List<GetTopAssistantsResponse>>(topAssistants);
    }
}
