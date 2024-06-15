using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using FootballManager.Application.Features.Shared.Responses;
using FootballManager.Application.Utilities;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetByIdWithMatchesHistory;

public record GetClubByIdWithMatchHistoryQuery(int Id, Pagination Pagination) : IRequest<Result<ListResponse<MatchResultResponse>>>;

public class GetClubWithMatchHistoryQueryHandler(IMatchRepository repository, IMapper mapper)
    : IRequestHandler<GetClubByIdWithMatchHistoryQuery, Result<ListResponse<MatchResultResponse>>>
{
    public async Task<Result<ListResponse<MatchResultResponse>>> Handle(
        GetClubByIdWithMatchHistoryQuery request, CancellationToken cancellationToken)
    {
        var (history, total) = await repository.GetMatchHistoryForClub(request.Id).Page(request.Pagination, cancellationToken);
        if (history is null)
        {
            return new NotFoundResult<ListResponse<MatchResultResponse>>($"Club with ID {request.Id} not found");
        }

        var result = mapper.Map<List<MatchResultResponse>>(history);

        return new SuccessResult<ListResponse<MatchResultResponse>>(new(result, total));
    }
}
