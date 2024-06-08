using FluentValidation;
using FootballManager.Application.Contracts.Persistence;

namespace FootballManager.Application.Features.Match.Commands.Simulate;

public class SimulateMatchCommandValidator : AbstractValidator<SimulateMatchCommand>
{
    private readonly IClubRepository _clubRepository;
    private readonly ISeasonRepository _seasonRepository;
    public SimulateMatchCommandValidator(IClubRepository clubRepository, ISeasonRepository seasonRepository)
    {
        _clubRepository = clubRepository;
        _seasonRepository = seasonRepository;

        RuleFor(c => c.HomeTeamId)
            .GreaterThan(0)
            .MustAsync(ClubExists)
            .WithMessage("Such home team does not exists");

        RuleFor(c => c.AwayTeamId)
            .GreaterThan(0)
            .MustAsync(ClubExists)
            .WithMessage("Such away team does not exists");

        RuleFor(c => c.SeasonId)
            .GreaterThan(0)
            .MustAsync(async (id, _) => await _seasonRepository.AnyAsync(s => s.Id == id))
            .WithMessage("Season must exists");
    }

    private async Task<bool> ClubExists(int clubId, CancellationToken cancellationToken)
    {
        return await _clubRepository.AnyAsync(c => c.Id == clubId);
    }
}
