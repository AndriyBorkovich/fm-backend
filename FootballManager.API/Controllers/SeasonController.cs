using FootballManager.Application.Features.Season.Queries.GetAllByChampionshipId;
using FootballManager.Application.Features.Season.Queries.GetTable;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;

namespace FootballManager.API.Controllers;
/// <inheritdoc/>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SeasonController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets the summary table for a specific season.
    /// </summary>
    /// <param name="seasonId">The ID of the chosen season.</param>
    /// <returns>
    /// An <see cref="ActionResult"/> containing a list of <see cref="GetSeasonTableResponse"/> objects 
    /// representing the summary table of the specified season. 
    /// The table includes team standings with details such as
    /// played games, wins, draws, losses, goals scored, 
    /// goals conceded, goal difference, and total points.
    /// </returns>
    /// <remarks>
    /// This endpoint retrieves the standings table for a specific season, 
    /// providing a summary of the performance of each team in that season.
    /// </remarks>
    [HttpGet("GetTable/{seasonId:int}")]
    public async Task<ActionResult<List<GetSeasonTableResponse>>> GetTable(int seasonId)
    {
        var result = await mediator.Send(new GetSeasonTableQuery(seasonId));

        return this.FromResult(result);
    }

    /// <summary>
    /// Get all seasons for specific champ
    /// </summary>
    /// <param name="championshipId">Choosen champ ID</param>
    /// <returns>List of IDs and years</returns>
    [HttpGet("GetChampionshipSeasons/{championshipId:int}")]
    public async Task<ActionResult<List<GetAllSeasonsResponse>>> GetSeasons(int championshipId)
    {
        var result = await mediator.Send(new GetAllSeasonsByChampionshipIdQuery(championshipId));

        return this.FromResult(result);
    }
}
