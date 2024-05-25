using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetClubWithMatchesHistory;

public class GetClubWithMatchHistoryQueryHandler : IRequestHandler<GetClubWithMatchHistoryQuery, Result<ClubWithMatchHistoryDTO>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMapper _mapper;

    public GetClubWithMatchHistoryQueryHandler(IClubRepository clubRepository, IMapper mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public async Task<Result<ClubWithMatchHistoryDTO>> Handle(GetClubWithMatchHistoryQuery request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetClubWithMatchHistory(request.Id);
        if (club is null)
        {
            return new NotFoundResult<ClubWithMatchHistoryDTO>($"Club with ID not {request.Id} found");
        }

        var result = _mapper.Map<ClubWithMatchHistoryDTO>(club);

        return new SuccessResult<ClubWithMatchHistoryDTO>(result);
    }
}
