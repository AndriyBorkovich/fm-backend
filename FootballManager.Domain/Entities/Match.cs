using FootballManager.Domain.Common;
using FootballManager.Domain.Enums;

namespace FootballManager.Domain.Entities;

public class Match : BaseEntity
{
    public DateTime MatchDate { get; set; }
    public MatchResult Result { get; set; }

    public int HomeTeamId { get; set; }
    public Club HomeTeam { get; set; }

    public int AwayTeamId { get; set; }
    public Club AwayTeam { get; set; }

    public int? SeasonId { get; set; }
    public Season? Season { get; set; }

    public List<GoalAction> Goals { get; set; }
    public List<Card> Cards { get; set; }

    /// <summary>
    /// Participants of the match
    /// </summary>
    public List<Player> Players { get; set; }

    /// <summary>
    /// Calculate the score of the match
    /// </summary>
    /// <returns>A tuple representing the score (home team goals, away team goals, result value)</returns>
    /// <seealso cref="MatchResult"/>
    public (int homeGoals, int awayGoals, MatchResult result) CalculateScore()
    {
        var homeGoals = 0;
        var awayGoals = 0;

        // Iterate through the goals scored in the match
        foreach (var goal in Goals)
        {
            if (goal.Scorer.ClubId != null && goal.Scorer.ClubId.Value == HomeTeamId)
            {
                homeGoals++;
            }
            else if (goal.Scorer.ClubId != null && goal.Scorer.ClubId.Value == AwayTeamId)
            {
                awayGoals++;
            }
        }

        Result = GetMatchResult(homeGoals, awayGoals);
        return (homeGoals, awayGoals, Result);
    }

    private MatchResult GetMatchResult(int homeGoals, int awayGoals)
    {
        if (homeGoals > awayGoals)
        {
            return MatchResult.HomeTeamWin;
        }

        return awayGoals > homeGoals ? MatchResult.AwayTeamWin : MatchResult.Draw;
    }
}
