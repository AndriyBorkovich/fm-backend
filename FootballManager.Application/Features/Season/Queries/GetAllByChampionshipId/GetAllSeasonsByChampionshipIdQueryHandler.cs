using FootballManager.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Season.Queries.GetAllByChampionshipId;

public record GetAllSeasonsByChampionshipIdQuery(int ChampionshipId) : IRequest<Result<List<GetAllSeasonsResponse>>>;

public class GetAllSeasonsResponse
{
    public int SeasonId { get; set; }
    public string Year { get; set; } = string.Empty;
}
public class GetAllSeasonsByChampionshipIdQueryHandler(ISeasonRepository repository)
    : IRequestHandler<GetAllSeasonsByChampionshipIdQuery, Result<List<GetAllSeasonsResponse>>>
{
    public async Task<Result<List<GetAllSeasonsResponse>>> Handle(GetAllSeasonsByChampionshipIdQuery request, CancellationToken cancellationToken)
    {
        var seasons = await repository.GetAll()
                                    .Where(s => s.ChampionshipId == request.ChampionshipId)
                                    .Select(s => new GetAllSeasonsResponse
                                    {
                                        SeasonId = s.Id,
                                        Year = $"{s.StartYear}/{s.EndYear}",
                                    })
                                    .AsNoTracking()
                                    .ToListAsync(cancellationToken);
        if (seasons == null || seasons.Count == 0)
        {
            return new NotFoundResult<List<GetAllSeasonsResponse>>($"Champ with ID {request.ChampionshipId} not found");
        }

        return new SuccessResult<List<GetAllSeasonsResponse>>(seasons);
    }
}
