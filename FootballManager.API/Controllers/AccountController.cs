using FootballManager.Application.Contracts.Identity;
using FootballManager.Application.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceResult;
using ServiceResult.ApiExtensions;

namespace FootballManager.API.Controllers;

/// <inheritdoc />
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var result = await _accountService.Register(request);

        return this.FromResult(result);
    }
}
