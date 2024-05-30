using FootballManager.Application.Features.Shared.Responses;

namespace FootballManager.Application.Features.Club.Queries.GetWithMatchesHistory;

public class GetClubWithMatchHistoryResponse
{
    public string Name { get; set; }
    public string StadiumName { get; set; }
    public List<MatchResultResponse> HomeMatchesResults { get; set; }
    public List<MatchResultResponse> AwayMatchesResults { get; set; }
}
