using FluentValidation;

namespace FootballManager.Application.Features.Player.Commands.Create;

public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .NotNull()
            .MinimumLength(2)
            .WithMessage("{PropertyName} must be longer than 2 characters")
            .MaximumLength(70)
            .WithMessage("{PropertyName} must be less than 70 characters");

        RuleFor(p => p.BirthDay)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .LessThan(DateTime.Now)
            .WithMessage("{PropertyName} can't be in future");

        RuleFor(c => c.Nationality)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(50)
            .WithMessage("{PropertyName} must be less than 50 characters");
    }
}
