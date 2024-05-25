using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetAllClubs;

public class GetAllClubsQueryHandler : IRequestHandler<GetAllClubsQuery, Result<List<ClubShortInfoDTO>>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMapper _mapper;

    public GetAllClubsQueryHandler(IClubRepository clubRepository, IMapper mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ClubShortInfoDTO>>> Handle(GetAllClubsQuery request, CancellationToken cancellationToken)
    {
        var clubs = _clubRepository
                                    .GetClubsWithPlayersInfo()
                                    .AsNoTracking();
        var clubsList = await clubs.ToListAsync(cancellationToken);

        var result = _mapper.Map<List<ClubShortInfoDTO>>(clubs);

        return new SuccessResult<List<ClubShortInfoDTO>>(result);
    }
}
