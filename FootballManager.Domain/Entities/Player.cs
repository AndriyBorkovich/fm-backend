using System.ComponentModel.DataAnnotations;
using FootballManager.Domain.Common;
using FootballManager.Domain.Enums;

namespace FootballManager.Domain.Entities;

public class Player : BaseFootballPerson
{
    public PlayerPosition Position { get; set; }

    public int? ClubId { get; set; }
    public Club? CurrentClub { get; set; }
    public List<GoalAction> ScoredGoals { get; set; }
    public List<GoalAction> AssistedGoals { get; set; }
    public List<Card> Cards { get; set; }

    /// <summary>
    /// Represents matches where player played
    /// </summary>
    public List<Match> Matches { get; set; }
}
