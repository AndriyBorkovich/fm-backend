using FluentValidation;
using FootballManager.Application.Contracts.Persistence;

namespace FootballManager.Application.Features.Coach.Commands.Create
{
    public class CreateCoachCommandValidator : AbstractValidator<CreateCoachCommand>
    {
        private readonly IClubRepository _clubRepository;
        public CreateCoachCommandValidator(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;

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

            RuleFor(c => c.ClubId)
                .MustAsync(async (id, _) => id == null || await _clubRepository.AnyAsync(c => c.Id == id))
                .WithMessage("Choosen club must exist!");

        }
    }
}
