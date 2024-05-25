using System.ComponentModel.DataAnnotations;
using FootballManager.Domain.Common;
using FootballManager.Domain.Enums;

namespace FootballManager.Domain.Entities;

public class Club : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(100)]
    public string StadiumName { get; set; }
    public ClubType Type { get; set; }

    public Coach? Coach { get; set; }
    [Length(11, 32)]
    public List<Player> Players { get; set; }
    public List<Match> HomeMatches { get; set; }
    public List<Match> AwayMatches { get; set; }
    public List<Championship> ParticipatingChampionships { get; set; }
}
