using Match = FootballManager.Domain.Entities.Match;

namespace FootballManager.Application.Contracts.Persistence;

public interface IMatchRepository : IGenericRepository<Match>
{
    IQueryable<Match> GetAllShortInfo();
    Task<List<Match>> GetMatchHistoryForClub(int clubId);
}
