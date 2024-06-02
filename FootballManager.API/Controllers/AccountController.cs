using FootballManager.Application.Contracts.Identity;
using FootballManager.Application.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;

namespace FootballManager.API.Controllers;

/// <inheritdoc />
[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">New user's data: first and last names, email and password</param>
    /// <returns>Result of the operation, and errors (if exist)</returns>
    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var result = await accountService.Register(request);

        return this.FromResult(result);
    }

    /// <summary>
    /// Login with existing user's credentials
    /// </summary>
    /// <param name="request">Contains username and password</param>
    /// <returns>Result of the operation, JWT or error message</returns>
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
        var result = await accountService.Login(request);

        return this.FromResult(result);
    }
}
