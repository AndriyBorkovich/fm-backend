using FootballManager.Domain.Enums;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Commands.Create;

public class CreatePlayerCommand : IRequest<Result<int>>
{
    public string Name { get; set; }
    public DateTime BirthDay { get; set; }
    public PlayerPosition Position { get; set; }
    public string Nationality { get; set; }
    public int? ClubId { get; set; }
}
