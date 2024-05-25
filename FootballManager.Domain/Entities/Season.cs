using FootballManager.Domain.Common;

namespace FootballManager.Domain.Entities;

public class Season : BaseEntity
{
    public int StartYear { get; set; }
    public int EndYear { get; set; }

    public int ChampionshipId { get; set; }
    public Championship Championship { get; set; }

    /// <summary>
    /// Matches played during season
    /// </summary>
    public List<Match> Matches { get; set; }
}
