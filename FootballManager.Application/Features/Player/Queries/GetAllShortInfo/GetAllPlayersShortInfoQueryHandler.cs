using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using FootballManager.Application.Utilities;
using LinqKit;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;
using PlayerEntity = FootballManager.Domain.Entities.Player;

namespace FootballManager.Application.Features.Player.Queries.GetAllShortInfo;

public record GetAllPlayersShortInfoQuery(int? ClubId, Pagination Pagination) : IRequest<Result<ListResponse<GetAllPlayersShortInfoResponse>>>;

public class GetAllPlayersShortInfoResponse
{
    public int Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public string Nationality { get; set; }
    public string Position { get; set; }
}

public class GetAllPlayersShortInfoQueryHandler(
    IMapper mapper,
    IAppLogger<GetAllPlayersShortInfoQueryHandler> logger,
    IClubRepository clubRepository,
    IPlayerRepository playerRepository)
        : IRequestHandler<GetAllPlayersShortInfoQuery, Result<ListResponse<GetAllPlayersShortInfoResponse>>>
{
    public async Task<Result<ListResponse<GetAllPlayersShortInfoResponse>>> Handle(GetAllPlayersShortInfoQuery request, CancellationToken cancellationToken)
    {
        var filters = PredicateBuilder.New<PlayerEntity>(true);
        var clubId = request.ClubId;
        if (clubId is not null && await clubRepository.AnyAsync(c => c.Id == clubId))
        {
            filters = filters.And(p => p.ClubId == clubId);
        }

        var (players, total) = await playerRepository
                                            .GetAll()
                                            .AsNoTracking()
                                            .Where(filters)
                                            .Page(request.Pagination, cancellationToken);

        logger.LogInformation("Players were retrieved successfully");

        var result = mapper.Map<List<GetAllPlayersShortInfoResponse>>(players);

        return new SuccessResult<ListResponse<GetAllPlayersShortInfoResponse>>(new(result, total));
    }
}
