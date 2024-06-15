using FootballManager.Application.Features.Club.Commands.Create;
using FootballManager.Application.Features.Club.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Club.Queries.GetByIdWithMatchesHistory;
using FootballManager.Application.Features.Club.Queries.GetByIdWithStats;
using FootballManager.Application.Features.Shared.Responses;
using FootballManager.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;

namespace FootballManager.API.Controllers;

/// <inheritdoc />
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ClubController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get clubs' short info
    /// </summary>
    /// <param name="query">Contains pagination data</param>
    /// <see cref="GetAllClubsShortInfoResponse"/>
    /// <returns>Info about club including name, type, stadium and coach info, players count</returns>
    [HttpGet("GetAllShortInfo")]
    public async Task<ActionResult<ListResponse<GetAllClubsShortInfoResponse>>> GetAll(
        [FromQuery] GetAllClubsShortInfoQuery query)
    {
        var result = await mediator.Send(query);

        return this.FromResult(result);
    }

    /// <summary>
    /// Get club info about played matches
    /// </summary>
    /// <param name="query">Contains club ID and pagination data</param>
    /// <returns>Info about club's matches <see cref="MatchResultResponse"/></returns>
    [HttpGet("GetMatchHistory")]
    public async Task<ActionResult<List<MatchResultResponse>>> GetByIdWithMatchHistory(
        [FromQuery] GetClubByIdWithMatchHistoryQuery query)
    {
        var result = await mediator.Send(query);

        return this.FromResult(result);
    }

    /// <summary>
    /// Get club stats
    /// </summary>
    /// <param name="id">ID of specific club</param>
    /// <returns>See <see cref="GetClubByIdWithStatsResponse"/></returns>
    [HttpGet("GetById/{id:int}")]
    public async Task<ActionResult<GetClubByIdWithStatsResponse>> GetById(int id)
    {
        var result = await mediator.Send(new GetClubByIdWithStatsQuery(id));

        return this.FromResult(result);
    }

    /// <summary>
    /// Create new club
    /// </summary>
    /// <param name="command">Info about new club</param>
    /// <returns>ID of created club</returns>
    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateClubCommand command)
    {
        var result = await mediator.Send(command);

        return this.FromResult(result);
    }
}
