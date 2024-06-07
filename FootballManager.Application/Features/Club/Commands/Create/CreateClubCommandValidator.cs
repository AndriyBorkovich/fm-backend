using FluentValidation;

namespace FootballManager.Application.Features.Club.Commands.Create;

public class CreateClubCommandValidator : AbstractValidator<CreateClubCommand>
{
    public CreateClubCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();

        RuleFor(c => c.StadiumName).NotEmpty();
    }
}
