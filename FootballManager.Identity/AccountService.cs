using FootballManager.Application.Contracts.Identity;
using FootballManager.Application.Models.Identity;
using FootballManager.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using ServiceResult;

namespace FootballManager.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public AccountService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
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

        return new SuccessResult<RegistrationResponse>(new RegistrationResponse() { IsSuccessfulRegistration = true });
    }

    public Task<Result<AuthResponse>> Login(AuthRequest request)
    {
        throw new NotImplementedException();
    }
}
