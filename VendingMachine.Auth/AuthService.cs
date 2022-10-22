using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using VendingMachine.Domain.User;

namespace VendingMachine.Auth;

public class AuthService : IAuthService, ICurrentUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<ITokenCredentials> Authenticate(LoginModel loginModel)
    {
        return _userRepository.GetCredentials(loginModel.UserName,loginModel.Password);
    }

    public async Task<ITokenCredentials> Register(RegisterModel registerModel)
    {
        var registerFn = registerModel.IsSeller
            ? _userRepository.CreateSeller(registerModel.UserName, registerModel.Password)
            : _userRepository.CreateBuyer(registerModel.UserName, registerModel.Password);
        await registerFn;

        return await Authenticate(registerModel);
    }

    public Task<ITokenCredentials> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        return _userRepository.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken, refreshTokenRequest.RefreshToken);
    }

    public string GetCurrentUserName()
    {
        var claims = this._httpContextAccessor.HttpContext.User.Claims.ToArray();
        var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (userName == null)
        {
            throw new NoUserInContextException();
        }

        return userName;
    }
}

public class NoUserInContextException : Exception
{
}
