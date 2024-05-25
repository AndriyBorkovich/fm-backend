using FootballManager.Application.Features.Club.Commands.Create;
using FootballManager.Application.Features.Club.Queries.GetAllClubs;
using FootballManager.Application.Features.Club.Queries.GetClubWithMatchesHistory;
using FootballManager.Application.Features.Player.Queries.GetAllPlayers;
using FootballManager.Application.Features.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;

namespace FootballManager.API.Controllers;

/// <inheritdoc />
[Route("api/[controller]")]
[ApiController]
public class ClubController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClubController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get clubs with info about their players
    /// </summary>
    /// <see cref="ClubShortInfoDTO"/>
    /// <seealso cref="PlayerShortInfoDTO"/>
    /// <returns>Info about club and it's players</returns>
    [HttpGet("GetAllClubs")]
    public async Task<ActionResult<List<ClubShortInfoDTO>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllClubsQuery());
        return this.FromResult(result);
    }

    /// <summary>
    /// Get club info about played matches
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <see cref="ClubWithMatchHistoryDTO"/>
    /// <seealso cref="MatchResultDTO"/>
    /// <returns>Info about club's matches</returns>
    [HttpGet("GetByIdWithMatchHistory/{id:int}")]
    public async Task<ActionResult<ClubWithMatchHistoryDTO>> GetByIdWithMatchHistory(int id)
    {
        var result = await _mediator.Send(new GetClubWithMatchHistoryQuery(id));

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
        var result = await _mediator.Send(command);

        return this.FromResult(result);
    }
}
