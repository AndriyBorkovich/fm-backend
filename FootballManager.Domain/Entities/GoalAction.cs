using System.ComponentModel.DataAnnotations;
using FootballManager.Domain.Common;

namespace FootballManager.Domain.Entities;

public class GoalAction : BaseEntity
{
    public int ScorerId { get; set; }
    public Player Scorer { get; set; }
    [Range(1, 120)]
    public int Minute { get; set; }
    
    public int? AssistantId { get; set; }
    public Player? Assistant { get; set; }
    
    public int MatchId { get; set; }
    public Match Match { get; set; }
    
    public bool IsOwnGoal { get; set; }
}