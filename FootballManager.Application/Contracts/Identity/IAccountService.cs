using FootballManager.Application.Models.Identity;
using ServiceResult;

namespace FootballManager.Application.Contracts.Identity;

public interface IAccountService
{
    Task<Result<RegistrationResponse>> Register(RegistrationRequest request);
    Task<Result<AuthResponse>> Login(AuthRequest request);
}
