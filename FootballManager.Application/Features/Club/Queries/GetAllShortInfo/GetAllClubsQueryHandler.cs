using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetAllShortInfo;

public record GetAllClubsQuery : IRequest<Result<List<GetAllClubShortInfoResponse>>>;

public class GetAllClubsQueryHandler : IRequestHandler<GetAllClubsQuery, Result<List<GetAllClubShortInfoResponse>>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMapper _mapper;

    public GetAllClubsQueryHandler(IClubRepository clubRepository, IMapper mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<GetAllClubShortInfoResponse>>> Handle(GetAllClubsQuery request, CancellationToken cancellationToken)
    {
        var clubs = _clubRepository
                                    .GetClubsWithPlayersInfo()
                                    .AsNoTracking();
        var clubsList = await clubs.ToListAsync(cancellationToken);

        var result = _mapper.Map<List<GetAllClubShortInfoResponse>>(clubs);

        return new SuccessResult<List<GetAllClubShortInfoResponse>>(result);
    }
}