using FluentValidation;
using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using ServiceResult;
using MatchEntity = FootballManager.Domain.Entities.Match;

namespace FootballManager.Application.Features.Match.Commands.Schedule;

public record ScheduleMatchCommand
(
    int HomeTeamId,
    int AwayTeamId,
    int? SeasonId,
    DateTime MatchDate
) : IRequest<Result<int>>;

public class ScheduleMatchCommandHandler(
    IMatchRepository matchRepository,
    IValidator<ScheduleMatchCommand> validator,
    IMapper mapper) : IRequestHandler<ScheduleMatchCommand, Result<int>>
{
    public async Task<Result<int>> Handle(ScheduleMatchCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new InvalidResult<int>(validationResult.ToString());
        }

        var match = mapper.Map<MatchEntity>(request)!;

        await matchRepository.InsertAsync(match);

        return new SuccessResult<int>(match.Id);
    }
}
