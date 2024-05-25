using FootballManager.Application.Features.Player.Queries.GetAllPlayers;

namespace FootballManager.Application.Features.Club.Queries.GetAllClubs;

public class ClubShortInfoDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public List<PlayerShortInfoDTO> Players { get; set; }
}
