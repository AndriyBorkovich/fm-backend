using FootballManager.Application.Features.Statistics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;

namespace FootballManager.API.Controllers
{
    /// <inheritdoc/>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StatisticsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Get tournament top scorers list for particular season
        /// </summary>
        /// <param name="seasonId">ID of the choosen season</param>
        /// <returns>List of the top scorers with ID, name, goals count</returns>
        [HttpGet("GetSeasonTopScorers/{seasonId:int}")]
        public async Task<ActionResult<List<GetSeasonTopScorersQuery>>> GetSeasonTopScorers(int seasonId)
        {
            var result = await mediator.Send(new GetSeasonTopScorersQuery(seasonId));

            return this.FromResult(result);
        }

        /// <summary>
        /// Get tournament top assistants list for particular season
        /// </summary>
        /// <param name="seasonId">ID of the choosen season</param>
        /// <returns>List of the top assistants with ID, name, assists count</returns>
        [HttpGet("GetSeasonTopAssistants/{seasonId:int}")]
        public async Task<ActionResult<List<GetTopAssistantsResponse>>> GetSeasonTopAssistants(int seasonId)
        {
            var result = await mediator.Send(new GetSeasonTopAssistantsQuery(seasonId));

            return this.FromResult(result);
        }
    }
}
