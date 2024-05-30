using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Queries.GetAllShortInfo;

public record GetAllPlayersQuery : IRequest<Result<List<GetAllPlayersShortInfoResponse>>>;
