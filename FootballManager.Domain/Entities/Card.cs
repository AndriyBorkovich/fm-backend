using System.ComponentModel.DataAnnotations;
using FootballManager.Domain.Common;
using FootballManager.Domain.Enums;

namespace FootballManager.Domain.Entities;

public class Card : BaseEntity
{
    public int PlayerId { get; set; }
    public Player Player { get; set; }

    [Range(1, 120)]
    public int Minute { get; set; }
    
    public CardType Type { get; set; }
    
    public int MatchId { get; set; }
    public Match Match { get; set; }
}