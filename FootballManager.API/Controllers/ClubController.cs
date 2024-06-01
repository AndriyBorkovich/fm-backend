using FootballManager.Application.Features.Club.Commands.Create;
using FootballManager.Application.Features.Club.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Club.Queries.GetWithMatchesHistory;
using FootballManager.Application.Features.Player.Queries.GetAllShortInfo;
using FootballManager.Application.Features.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;

namespace FootballManager.API.Controllers;

/// <inheritdoc />
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ClubController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClubController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get clubs' short info
    /// </summary>
    /// <see cref="GetAllClubsShortInfoResponse"/>
    /// <returns>Info about club including name, type, stadium and coach info, players count</returns>
    [HttpGet("GetAllShortInfo")]
    public async Task<ActionResult<List<GetAllClubsShortInfoResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllClubsShortInfoQuery());
        return this.FromResult(result);
    }

    /// <summary>
    /// Get club info about played matches
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <see cref="GetClubWithMatchHistoryResponse"/>
    /// <seealso cref="MatchResultResponse"/>
    /// <returns>Info about club's matches</returns>
    [HttpGet("GetWithMatchHistory/{id:int}")]
    public async Task<ActionResult<GetClubWithMatchHistoryResponse>> GetByIdWithMatchHistory(int id)
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
