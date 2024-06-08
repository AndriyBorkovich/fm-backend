using System.ComponentModel.DataAnnotations;
using FootballManager.Domain.Common;
using FootballManager.Domain.Enums;

namespace FootballManager.Domain.Entities;

public class Championship : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(100)]
    public string Country { get; set; }
    public ChampionshipType Type { get; set; }

    public List<Season> Seasons { get; set; }
    public List<Club> ParticipatingClubs { get; set; }
}
