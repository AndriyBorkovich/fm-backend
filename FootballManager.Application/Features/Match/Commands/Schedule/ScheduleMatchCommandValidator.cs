using FluentValidation;
using FootballManager.Application.Contracts.Persistence;

namespace FootballManager.Application.Features.Match.Commands.Schedule
{
    public class ScheduleMatchCommandValidator : AbstractValidator<ScheduleMatchCommand>
    {
        private readonly IClubRepository _clubRepository;
        private readonly ISeasonRepository _seasonRepository;
        public ScheduleMatchCommandValidator(IClubRepository clubRepository, ISeasonRepository seasonRepository)
        {
            _clubRepository = clubRepository;
            _seasonRepository = seasonRepository;

            RuleFor(c => c.MatchDate)
                .NotEmpty()
                .GreaterThan(DateTime.Now)
                .WithMessage("Scheduled date cannot be in the past");

            RuleFor(c => c.HomeTeamId)
                .MustAsync(ClubExists)
                .WithMessage("Such home team does not exists");

            RuleFor(c => c.AwayTeamId)
                .MustAsync(ClubExists)
                .WithMessage("Such away team does not exists");

            RuleFor(c => c.SeasonId)
                .MustAsync(async (id, _) => id == null || await _seasonRepository.AnyAsync(s => s.Id == id))
                .WithMessage("Choosen season must exists!");
        }

        private async Task<bool> ClubExists(int clubId, CancellationToken cancellationToken)
        {
            return await _clubRepository.AnyAsync(c => c.Id == clubId);
        }
    }
}
