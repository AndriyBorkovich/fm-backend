using System.IdentityModel.Tokens.Jwt;
using FootballManager.Application.Contracts.Identity;
using FootballManager.Application.Models.Identity;
using FootballManager.Domain.Entities;
using FootballManager.Identity.Handlers;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using ServiceResult;

namespace FootballManager.Identity.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly JwtHandler _jwtHandler;

    public AccountService(UserManager<AppUser> userManager, IMapper mapper, JwtHandler jwtHandler)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtHandler = jwtHandler;
    }

    public async Task<Result<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var user = _mapper.Map<AppUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);

            return new SuccessResult<RegistrationResponse>(new RegistrationResponse()
            {
                IsSuccessfulRegistration = false,
                Errors = errors
            });
        }

        await _userManager.AddToRoleAsync(user, Roles.Admin);

        return new SuccessResult<RegistrationResponse>(new RegistrationResponse()
        {
            IsSuccessfulRegistration = true
        });
    }

    public async Task<Result<AuthResponse>> Login(AuthRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new SuccessResult<AuthResponse>(new AuthResponse
            {
                IsAuthSuccessful = false,
                ErrorMessage = "Invalid authentication"
            });
        }

        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var claims = await _jwtHandler.GetClaims(user);
        var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new SuccessResult<AuthResponse>(new AuthResponse
        {
            IsAuthSuccessful = true,
            Token = token
        });
    }
}
