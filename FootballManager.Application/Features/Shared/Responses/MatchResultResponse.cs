using System.Text.Json.Serialization;

namespace FootballManager.Application.Features.Shared.Responses;

public class MatchResultResponse
{
    public int MatchId { get; set; }
    public string Date { get; set; }
    public string HomeTeamName { get; set; }
    public int HomeTeamGoals { get; set; }

    public string AwayTeamName { get; set; }
    public int AwayTeamGoals { get; set; }

    public string Result { get; set; }
}
