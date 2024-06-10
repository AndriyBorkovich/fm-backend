using FootballManager.Application.Features.Match.Commands.Schedule;
using FootballManager.Application.Features.Match.Commands.Simulate;
using FootballManager.Application.Features.Match.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Match.Queries.GetById;
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
public class MatchController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Simulate match between two teams
    /// </summary>
    /// <param name="command">IDs of home and away teams</param>
    /// <returns>Score of the match</returns>
    [HttpPost("Simulate")]
    public async Task<ActionResult<MatchResultResponse>> Simulate(SimulateMatchCommand command)
    {
        var result = await mediator.Send(command);

        return this.FromResult(result);
    }

    /// <summary>
    /// Schedule (create) new match
    /// </summary>
    /// <param name="command">Contains data about teams, season, date</param>
    /// <returns>ID of the created entity</returns>
    [HttpPost("Schedule")]
    public async Task<ActionResult<int>> Schedule(ScheduleMatchCommand command)
    {
        var result = await mediator.Send(command);

        return this.FromResult(result);
    }

    /// <summary>
    /// Get all matches results
    /// </summary>
    /// <param name="query">Contains date filters, pagination params</param>
    /// <returns>List with matches info results</returns>
    [HttpGet("GetAllShortInfo")]
    public async Task<ActionResult<ListResponse<GetAllMatchesShortInfoResponse>>> GetAllShortInfo(
        [FromQuery] GetAllMatchesShortInfoQuery query)
    {
        return await mediator.Send(query);
    }

    /// <summary>
    /// Get match events
    /// </summary>
    /// <param name="id">Id of the specific match</param>
    /// <returns>Data about match result, teams and match events</returns>
    [HttpGet("GetById/{id:int}")]
    public async Task<ActionResult<GetMatchByIdResponse>> GetById(int id)
    {
        var result = await mediator.Send(new GetMatchByIdQuery(id));

        return this.FromResult(result);
    }
}
