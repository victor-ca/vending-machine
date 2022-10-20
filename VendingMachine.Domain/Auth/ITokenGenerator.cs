using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VendingMachine.Domain.Auth;

public interface ITokenGenerator
{
    JwtSecurityToken CreateToken(List<Claim> authClaims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
}