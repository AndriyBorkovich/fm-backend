using FluentValidation;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using ServiceResult;
using ClubEntity = FootballManager.Domain.Entities.Club;

namespace FootballManager.Application.Features.Club.Commands.Create;

public record CreateClubCommand
(
    string Name,
    string StadiumName,
    ClubType Type
) : IRequest<Result<int>>;

public class CreateClubCommandHandler(
    IValidator<CreateClubCommand> validator,
    IClubRepository clubRepository,
    IMapper mapper)
        : IRequestHandler<CreateClubCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateClubCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new InvalidResult<int>(validationResult.ToString());
        }

        var club = mapper.Map<ClubEntity>(request);

        await clubRepository.InsertAsync(club);

        return new SuccessResult<int>(club.Id);
    }
}
