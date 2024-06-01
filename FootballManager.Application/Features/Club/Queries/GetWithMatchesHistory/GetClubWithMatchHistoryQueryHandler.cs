using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Features.Shared.Responses;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetWithMatchesHistory;

public record GetClubWithMatchHistoryQuery(int Id) : IRequest<Result<List<MatchResultResponse>>>;

public class GetClubWithMatchHistoryQueryHandler(IMatchRepository repository, IMapper mapper)
    : IRequestHandler<GetClubWithMatchHistoryQuery, Result<List<MatchResultResponse>>>
{
    public async Task<Result<List<MatchResultResponse>>> Handle(GetClubWithMatchHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await repository.GetMatchHistoryForClub(request.Id);
        if (history is null)
        {
            return new NotFoundResult<List<MatchResultResponse>>($"Club with ID {request.Id} not found");
        }

        var result = mapper.Map<List<MatchResultResponse>>(history);

        return new SuccessResult<List<MatchResultResponse>>(result);
    }
}
