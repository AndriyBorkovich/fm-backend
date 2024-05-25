using FootballManager.Domain.Enums;

namespace FootballManager.Application.Features.Player.Queries.GetPlayerWithStats;

public class PlayerStatisticDTO
{
    public string Name { get; set; }
    public string ClubName { get; set; }
    public string Position { get; set; }
    public int Age { get; set; }
    public int  MatchesPlayed { get; set; }
    public int GoalsScored { get; set; }
    public int Assists { get; set; }
    public int RedCards { get; set; }
    public int YellowCards { get; set; }
}