using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Match.Queries.GetById;

public record GetMatchByIdQuery(int MatchId) : IRequest<Result<GetMatchByIdResponse>>;
public class GetMatchByIdQueryHandler(
    IMatchRepository repository,
    IMapper mapper)
        : IRequestHandler<GetMatchByIdQuery, Result<GetMatchByIdResponse>>
{
    public async Task<Result<GetMatchByIdResponse>> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
    {
        var match = await repository.GetAll()
                                          .Include(m => m.HomeTeam)
                                          .Include(m => m.AwayTeam)
                                          .Include(m => m.Goals.OrderBy(g => g.Minute))
                                              .ThenInclude(g => g.Scorer)
                                          .Include(m => m.Goals.OrderBy(g => g.Minute))
                                              .ThenInclude(g => g.Assistant)
                                          .Include(m => m.Cards.OrderBy(g => g.Minute))
                                              .ThenInclude(c => c.Player)
                                          .AsSplitQuery()
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(m => m.Id == request.MatchId, cancellationToken);

        if (match == null)
        {
            return new NotFoundResult<GetMatchByIdResponse>($"Match with ID {request.MatchId} not found");
        }

        var response = mapper.Map<GetMatchByIdResponse>(match);

        return new SuccessResult<GetMatchByIdResponse>(response);
    }
}
