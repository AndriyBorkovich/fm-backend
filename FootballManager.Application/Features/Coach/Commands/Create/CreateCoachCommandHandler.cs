using FluentValidation;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using ServiceResult;
using CoachEntity = FootballManager.Domain.Entities.Coach;


namespace FootballManager.Application.Features.Coach.Commands.Create;

public record CreateCoachCommand
(
    string Name,
    string Nationality,
    DateTime BirthDay,
    CoachingStyle CoachingStyle,
    int? ClubId
) : IRequest<Result<int>>;

public class CreateCoachCommandHandler(
    IValidator<CreateCoachCommand> validator,
    IMapper mapper,
    ICoachRepository repository)
        : IRequestHandler<CreateCoachCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateCoachCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new InvalidResult<int>(validationResult.ToString());
        }

        var coach = mapper.Map<CoachEntity>(request);

        await repository.InsertAsync(coach);

        return new SuccessResult<int>(coach.Id);
    }
}
