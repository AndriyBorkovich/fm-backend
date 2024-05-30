using FootballManager.Application.Contracts.Persistence;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Commands.Delete;

public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand, Result<Unit>>
{
    private readonly IPlayerRepository _playerRepository;

    public DeletePlayerCommandHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Result<Unit>> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var playerToDelete = await _playerRepository.GetByIdAsync(request.Id);
        if (playerToDelete is null)
        {
            return new NotFoundResult<Unit>($"Player with ID {request.Id} not found");
        }

        await _playerRepository.DeleteAsync(playerToDelete);

        return new SuccessResult<Unit>(Unit.Value);
    }
}
