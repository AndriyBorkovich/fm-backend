using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using FootballManager.Application.Features.Shared.Responses;
using FootballManager.Application.Utilities;
using LinqKit;
using MapsterMapper;
using MediatR;
using MatchEntity = FootballManager.Domain.Entities.Match;

namespace FootballManager.Application.Features.Match.Queries.GetAllShortInfo;

public record GetAllMatchesShortInfoQuery
(
    DateTime? StartDate,
    DateTime? EndDate,
    Pagination Pagination
) : IRequest<ListResponse<GetAllMatchesShortInfoResponse>>;

public class GetAllMatchesShortInfoResponse
{
    public required MatchResultResponse Match { get; set; }
    public bool IsLive { get; set; }
}

public class GetAllMatchesShortInfoQueryHandler(
    IMatchRepository repository,
    IMapper mapper)
    : IRequestHandler<GetAllMatchesShortInfoQuery, ListResponse<GetAllMatchesShortInfoResponse>>
{
    public async Task<ListResponse<GetAllMatchesShortInfoResponse>> Handle(GetAllMatchesShortInfoQuery request, CancellationToken cancellationToken)
    {
        var query = repository.GetAllShortInfo();

        var filters = PredicateBuilder.New<MatchEntity>(true);
        var startDate = request.StartDate ?? DateTime.UtcNow.Date;
        filters = filters.And(m => m.MatchDate >= startDate);

        if (request.EndDate is not null)
        {
            filters = filters.And(m => m.MatchDate <= request.EndDate);
        }

        var (items, total) = await query.Where(filters).Page(request.Pagination, cancellationToken);

        var result = mapper.Map<List<GetAllMatchesShortInfoResponse>>(items);

        return new ListResponse<GetAllMatchesShortInfoResponse>(result, total);
    }
}
