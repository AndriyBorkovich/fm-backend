using FootballManager.Application.Features.Match.Commands.Simulate;
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
public class MatchController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Simulate match between two teams
    /// </summary>
    /// <param name="command">IDs of home and away teams</param>
    /// <returns>Score of the match</returns>
    [HttpPost("Simulate")]
    public async Task<ActionResult<MatchResultResponse>> SimulateMatch(SimulateMatchCommand command)
    {
        var result = await _mediator.Send(command);

        return this.FromResult(result);
    }
}
