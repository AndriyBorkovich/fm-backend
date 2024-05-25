using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Queries.GetPlayerWithStats;

public record GetPlayerWithStatsQuery(int Id) : IRequest<Result<PlayerStatisticDTO>>;
