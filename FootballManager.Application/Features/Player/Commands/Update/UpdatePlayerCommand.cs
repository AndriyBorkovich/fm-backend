using FootballManager.Domain.Enums;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Commands.Update;

public class UpdatePlayerCommand : IRequest<Result<Unit>>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PlayerPosition Position { get; set; }
    public string Nationality { get; set; }
    public int? ClubId { get; set; }
}
