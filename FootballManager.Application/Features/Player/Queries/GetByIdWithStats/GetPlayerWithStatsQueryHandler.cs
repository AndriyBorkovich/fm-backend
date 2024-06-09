using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Queries.GetPlayerWithStats;

public record GetPlayerWithStatsQuery(int Id) : IRequest<Result<GetPlayerWithStatsResponse>>;

public class GetPlayerWithStatsQueryHandler(IMapper mapper, IPlayerRepository playerRepository) : IRequestHandler<GetPlayerWithStatsQuery, Result<GetPlayerWithStatsResponse>>
{
    public async Task<Result<GetPlayerWithStatsResponse>> Handle(GetPlayerWithStatsQuery request, CancellationToken cancellationToken)
    {
        var playerExists = await playerRepository.AnyAsync(p => p.Id == request.Id);
        if (!playerExists)
        {
            return new NotFoundResult<GetPlayerWithStatsResponse>($"Player with ID {request.Id} not found");
        }

        var player = await playerRepository.GetPlayerWithStats(request.Id);

        return new SuccessResult<GetPlayerWithStatsResponse>(mapper.Map<GetPlayerWithStatsResponse>(player!));
    }
}
