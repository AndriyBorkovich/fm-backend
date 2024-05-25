using FootballManager.Domain.Enums;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Commands.Create;

public class CreateClubCommand : IRequest<Result<int>>
{
    public string Name { get; set; }
    public string StadiumName { get; set; }
    public ClubType Type { get; set; }
}
