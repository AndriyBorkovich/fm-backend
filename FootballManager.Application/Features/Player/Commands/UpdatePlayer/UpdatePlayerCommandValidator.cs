using FluentValidation;
using FootballManager.Application.Contracts.Persistence;

namespace FootballManager.Application.Features.Player.Commands.UpdatePlayer;

public class UpdatePlayerCommandValidator: AbstractValidator<UpdatePlayerCommand>
{
    private readonly IPlayerRepository _playerRepository;

    public UpdatePlayerCommandValidator(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;

        RuleFor(c => c)
            .MustAsync(PlayerExistsAsync)
            .WithMessage("Player does not exists");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(50)
            .WithMessage("{PropertyName} must be less than 50 characters");

        RuleFor(c => c.Nationality)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(50)
            .WithMessage("{PropertyName} must be less than 50 characters");
    }

    private async Task<bool> PlayerExistsAsync(UpdatePlayerCommand command, CancellationToken cancellationToken)
    {
        return await _playerRepository.AnyAsync(p => p.Id == command.Id);
    }
}
