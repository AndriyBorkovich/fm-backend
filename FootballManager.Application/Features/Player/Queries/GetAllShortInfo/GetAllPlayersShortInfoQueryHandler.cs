using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using FootballManager.Application.Utilities;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Queries.GetAllShortInfo;

public record GetAllPlayersShortInfoQuery(Pagination Pagination) : IRequest<Result<ListResponse<GetAllPlayersShortInfoResponse>>>;

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
    IPlayerRepository playerRepository)
        : IRequestHandler<GetAllPlayersShortInfoQuery, Result<ListResponse<GetAllPlayersShortInfoResponse>>>
{
    public async Task<Result<ListResponse<GetAllPlayersShortInfoResponse>>> Handle(GetAllPlayersShortInfoQuery request, CancellationToken cancellationToken)
    {
        var (players, total) = await playerRepository.GetAll().AsNoTracking().Page(request.Pagination, cancellationToken);

        logger.LogInformation("Players were retrieved successfully");

        var result = mapper.Map<List<GetAllPlayersShortInfoResponse>>(players);

        return new SuccessResult<ListResponse<GetAllPlayersShortInfoResponse>>(new(result, total));
    }
}
