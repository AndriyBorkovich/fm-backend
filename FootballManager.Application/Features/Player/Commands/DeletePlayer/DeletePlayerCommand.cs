using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Commands.DeletePlayer;

public record DeletePlayerCommand(int Id) : IRequest<Result<Unit>>;
