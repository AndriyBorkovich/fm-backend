using FootballManager.Domain.Common;
using FootballManager.Domain.Enums;

namespace FootballManager.Domain.Entities;

public class Coach : BaseFootballPerson
{
    public CoachingStyle CoachingStyle { get; set; }

    public int? ClubId { get; set; }
    public Club? CurrentClub { get; set; }
}
