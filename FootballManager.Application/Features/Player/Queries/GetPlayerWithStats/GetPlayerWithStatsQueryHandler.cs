using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Queries.GetPlayerWithStats;

public class GetPlayerWithStatsQueryHandler : IRequestHandler<GetPlayerWithStatsQuery, Result<PlayerStatisticDTO>>
{
    private readonly IMapper _mapper;
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerWithStatsQueryHandler(IMapper mapper, IPlayerRepository playerRepository)
    {
        _mapper = mapper;
        _playerRepository = playerRepository;
    }

    public async Task<Result<PlayerStatisticDTO>> Handle(GetPlayerWithStatsQuery request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(request.Id);
        if (player is null)
        {
            return new NotFoundResult<PlayerStatisticDTO>($"Player with ID {request.Id} not found");
        }

        player = await _playerRepository.GetPlayerWithStats(request.Id);

        return new SuccessResult<PlayerStatisticDTO>(_mapper.Map<PlayerStatisticDTO>(player));
    }
}
