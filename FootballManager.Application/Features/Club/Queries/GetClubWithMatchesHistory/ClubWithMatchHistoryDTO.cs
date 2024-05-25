using FootballManager.Application.Features.Shared.DTOs;

namespace FootballManager.Application.Features.Club.Queries.GetClubWithMatchesHistory;

public class ClubWithMatchHistoryDTO
{
    public string Name { get; set; }
    public string StadiumName { get; set; }
    public List<MatchResultDTO> HomeMatchesResults { get; set; }
    public List<MatchResultDTO> AwayMatchesResults { get; set; }
}
