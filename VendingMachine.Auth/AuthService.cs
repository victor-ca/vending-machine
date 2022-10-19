using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendingMachine.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using VendingMachine.Domain.User;

namespace VendingMachine.Auth;

public class AuthService : IAuthService,ICurrentUserService
{
    private readonly AuthConfig _authConfig;
    private readonly IUserService _userSvc;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(AuthConfig authConfig, IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _authConfig = authConfig;
        _userSvc = userService;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<AuthenticateResult> Authenticate(string username, string password)
    {
        var credentials = await _userSvc.GetCredentialsByUsername(username);
        if (!AuthenticationUtils.VerifyPasswordHash(password, credentials.PasswordHash, credentials.PasswordSalt))
        {
            throw new Exception();
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authConfig.HashingSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("test","test")
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return new AuthenticateResult
        {
            Token = tokenHandler.WriteToken(token)
        };
    }

    public async Task<AuthenticateResult> CreateUser(string userName,string password)
    {
        AuthenticationUtils.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

       await _userSvc.CreateUser(
            new VendingMachineUser{IsVendor = false,UserName = userName},
            new VendingMachineUserCredentials { PasswordHash = passwordHash, PasswordSalt = passwordSalt });
        
        return await Authenticate(userName,password);
    }

    public string GetCurrentUserId()
    {
        var claims = this._httpContextAccessor.HttpContext.User.Claims.ToArray();
        var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (userId==null)
        {
            throw new Exception();
        }

        return userId;
    }
}