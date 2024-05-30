using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Queries.GetAllShortInfo;

public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, Result<List<GetAllPlayersShortInfoResponse>>>
{
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetAllPlayersQueryHandler> _logger;
    private readonly IPlayerRepository _playerRepository;

    public GetAllPlayersQueryHandler(
        IMapper mapper,
        IAppLogger<GetAllPlayersQueryHandler> logger,
        IPlayerRepository playerRepository)
    {
        _mapper = mapper;
        _logger = logger;
        _playerRepository = playerRepository;
    }

    public async Task<Result<List<GetAllPlayersShortInfoResponse>>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
    {
        var players = await _playerRepository.GetPlayersShortInfo();

        _logger.LogInformation("Players were retrieved successfully");

        return new SuccessResult<List<GetAllPlayersShortInfoResponse>>(_mapper.Map<List<GetAllPlayersShortInfoResponse>>(players));
    }
}
