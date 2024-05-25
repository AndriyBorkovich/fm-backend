using FootballManager.Domain.Common;
using FootballManager.Domain.Enums;

namespace FootballManager.Domain.Entities;

public class Championship : BaseEntity
{
    public ChampionshipType Type { get; set; }

    public List<Season> Seasons { get; set; }
    public List<Club> ParticipatingClubs { get; set; }
}
