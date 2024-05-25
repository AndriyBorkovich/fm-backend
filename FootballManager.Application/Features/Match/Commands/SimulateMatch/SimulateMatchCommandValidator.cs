using FluentValidation;
using FootballManager.Application.Contracts.Persistence;

namespace FootballManager.Application.Features.Match.Commands.SimulateMatch;

public class SimulateMatchCommandValidator : AbstractValidator<SimulateMatchCommand>
{
    private readonly IClubRepository _clubRepository;
    public SimulateMatchCommandValidator(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;

        RuleFor(c => c.HomeTeamId)
            .MustAsync(ClubExists)
            .WithMessage("Such home team does not exists");

        RuleFor(c => c.AwayTeamId)
            .MustAsync(ClubExists)
            .WithMessage("Such away team does not exists");
    }

    private async Task<bool> ClubExists(int clubId, CancellationToken cancellationToken)
    {
        return await _clubRepository.AnyAsync(c => c.Id == clubId);
    }
}
