using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Queries.GetPlayerWithStats;

public record GetPlayerWithStatsQuery(int Id) : IRequest<Result<GetPlayerWithStatsResponse>>;

public class GetPlayerWithStatsQueryHandler : IRequestHandler<GetPlayerWithStatsQuery, Result<GetPlayerWithStatsResponse>>
{
    private readonly IMapper _mapper;
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerWithStatsQueryHandler(IMapper mapper, IPlayerRepository playerRepository)
    {
        _mapper = mapper;
        _playerRepository = playerRepository;
    }

    public async Task<Result<GetPlayerWithStatsResponse>> Handle(GetPlayerWithStatsQuery request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(request.Id);
        if (player is null)
        {
            return new NotFoundResult<GetPlayerWithStatsResponse>($"Player with ID {request.Id} not found");
        }

        player = await _playerRepository.GetPlayerWithStats(request.Id);

        return new SuccessResult<GetPlayerWithStatsResponse>(_mapper.Map<GetPlayerWithStatsResponse>(player));
    }
}
