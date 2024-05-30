using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Commands.Delete;

public record DeletePlayerCommand(int Id) : IRequest<Result<Unit>>;
