using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Queries.GetAllPlayers;

public record GetAllPlayersQuery : IRequest<Result<List<PlayerShortInfoDTO>>>;
