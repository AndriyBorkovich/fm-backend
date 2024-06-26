using FootballManager.Application.Features.Player.Commands.Create;
using FootballManager.Application.Features.Player.Commands.Delete;
using FootballManager.Application.Features.Player.Commands.Update;
using FootballManager.Application.Features.Player.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Player.Queries.GetPlayerWithStats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;

namespace FootballManager.API.Controllers;

/// <inheritdoc />
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class PlayerController(IMediator mediator) : Controller
{
    /// <summary>
    /// Get all players
    /// </summary>
    /// <param name="query">Contains pagination data</param>
    /// <returns>List of players short info (name, age, position)</returns>
    [HttpGet("GetAllShortInfo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GetAllPlayersShortInfoResponse>>> GetAllShortInfo(
        [FromQuery] GetAllPlayersShortInfoQuery query)
    {
        var result = await mediator.Send(query);

        return this.FromResult(result);
    }

    /// <summary>
    /// Get player with statistics info
    /// </summary>
    /// <param name="id">Player ID</param>
    /// <returns>Info about player (matches played, goals, assists, cards)</returns>
    [HttpGet("GetById/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ActionResult), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetPlayerWithStatsResponse>> GetById(int id)
    {
        var result = await mediator.Send(new GetPlayerWithStatsQuery(id));

        return this.FromResult(result);
    }

    /// <summary>
    /// Create player
    /// </summary>
    /// <param name="command">Data to create player (name, birthday, position)</param>
    /// <returns>ID of created player</returns>
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Create(CreatePlayerCommand command)
    {
        var result = await mediator.Send(command);

        return this.FromResult(result);
    }

    /// <summary>
    /// Update player info
    /// </summary>
    /// <param name="command">Info to update specific player</param>
    /// <returns>Nothing</returns>
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(UpdatePlayerCommand command)
    {
        await mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Delete specific player
    /// </summary>
    /// <param name="id">ID of the player to delete</param>
    /// <returns>Nothing</returns>
    [HttpDelete("Delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(int id)
    {
        await mediator.Send(new DeletePlayerCommand(id));

        return NoContent();
    }
}
