using FootballManager.Application.Features.Player.Queries.GetAllShortInfo;

namespace FootballManager.Application.Features.Club.Queries.GetAllShortInfo;

public class GetAllClubShortInfoResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public List<GetAllPlayersShortInfoResponse> Players { get; set; }
}
