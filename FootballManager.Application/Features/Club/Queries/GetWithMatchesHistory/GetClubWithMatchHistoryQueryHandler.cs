using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetWithMatchesHistory;

public record GetClubWithMatchHistoryQuery(int Id) : IRequest<Result<GetClubWithMatchHistoryResponse>>;

public class GetClubWithMatchHistoryQueryHandler : IRequestHandler<GetClubWithMatchHistoryQuery, Result<GetClubWithMatchHistoryResponse>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMapper _mapper;

    public GetClubWithMatchHistoryQueryHandler(IClubRepository clubRepository, IMapper mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public async Task<Result<GetClubWithMatchHistoryResponse>> Handle(GetClubWithMatchHistoryQuery request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetClubWithMatchHistory(request.Id);
        if (club is null)
        {
            return new NotFoundResult<GetClubWithMatchHistoryResponse>($"Club with ID not {request.Id} found");
        }

        var result = _mapper.Map<GetClubWithMatchHistoryResponse>(club);

        return new SuccessResult<GetClubWithMatchHistoryResponse>(result);
    }
}
