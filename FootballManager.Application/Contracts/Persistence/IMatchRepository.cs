using Match = FootballManager.Domain.Entities.Match;

namespace FootballManager.Application.Contracts.Persistence;

public interface IMatchRepository : IGenericRepository<Match>
{
    IQueryable<Match> GetAllShortInfo();
    IQueryable<Match> GetMatchHistoryForClub(int clubId);
    void RegisterPlayerInMatch(int matchId, int playerId);
}
