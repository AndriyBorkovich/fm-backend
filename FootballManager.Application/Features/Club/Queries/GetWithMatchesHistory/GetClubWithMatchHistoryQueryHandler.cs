using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Features.Shared.Responses;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetWithMatchesHistory;

public record GetClubWithMatchHistoryQuery(int Id) : IRequest<Result<GetClubWithMatchHistoryResponse>>;

public class GetClubWithMatchHistoryResponse
{
    public List<MatchResultResponse> HomeMatchesResults { get; set; }
    public List<MatchResultResponse> AwayMatchesResults { get; set; }
}


public class GetClubWithMatchHistoryQueryHandler(IClubRepository repository, IMapper mapper)
    : IRequestHandler<GetClubWithMatchHistoryQuery, Result<GetClubWithMatchHistoryResponse>>
{
    public async Task<Result<GetClubWithMatchHistoryResponse>> Handle(GetClubWithMatchHistoryQuery request, CancellationToken cancellationToken)
    {
        var club = await repository.GetClubWithMatchHistory(request.Id);
        if (club is null)
        {
            return new NotFoundResult<GetClubWithMatchHistoryResponse>($"Club with ID {request.Id} not found");
        }

        var result = mapper.Map<GetClubWithMatchHistoryResponse>(club);

        return new SuccessResult<GetClubWithMatchHistoryResponse>(result);
    }
}
