using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using VendingMachine.Auth;
using VendingMachine.Domain.Auth;
using VendingMachine.Domain.User;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace VendingMachine.EF.Users;

public class EfUserRepository: IUserRepository
{
    
    private readonly UserManager<VendingMachineUserDpo> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly TokenGeneratorConfig _config;

    public EfUserRepository(
        UserManager<VendingMachineUserDpo> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenGenerator tokenGenerator,
        TokenGeneratorConfig config
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenGenerator = tokenGenerator;
        _config = config;
    }
    public async Task<ITokenCredentials> GetCredentials(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var areCredsValid = user != null && await _userManager.CheckPasswordAsync(user, password); 
        if (!areCredsValid)
        {
            throw new InvalidCredentialException();
        }
        var userRoles = await _userManager.GetRolesAsync(user!);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user!.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        authClaims.Add(new Claim("userName",userName));
        bool isSeller = userRoles.FirstOrDefault(x => x == UserRoles.Seller) !=  null; 
        authClaims.Add(new Claim("isSeller",isSeller?"true":"false"));
        
        var token = _tokenGenerator.CreateToken(authClaims);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_config.RefreshTokenValidityInDays);
        await _userManager.UpdateAsync(user);
        
        return new TokenCredentials
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        };
    }

    public async Task<ITokenCredentials> GetPrincipalFromExpiredToken(string accessToken, string refreshToken)
    {
        
        var principal = _tokenGenerator.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
          throw new Exception("Invalid access token or refresh token");
        }
        string username = principal.Identity!.Name!;
        var user = await _userManager.FindByNameAsync(username);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new Exception("Invalid access token or refresh token");
        }
        var newAccessToken = _tokenGenerator.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);
        return new TokenCredentials
        {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = refreshToken,
            Expiration = newAccessToken.ValidTo
        };
    }

    public async Task CreateBuyer(string userName, string password)
    {
      var user = await CreateUser(userName, password);
      await AssignRole(user, UserRoles.Buyer);
    }

    public async Task CreateSeller(string userName, string password)
    {
        var user = await CreateUser(userName, password);
        await AssignRole(user, UserRoles.Seller);
    }

    private async Task AssignRole(VendingMachineUserDpo user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole(role));
        }
        await _userManager.AddToRoleAsync(user, role);
    }

    private async Task<VendingMachineUserDpo> CreateUser(string userName, string password)
    {
        var userExists = await _userManager.FindByNameAsync(userName);
        if (userExists != null)
        {
            throw new UserAlreadyExistException(userName);
        }

        VendingMachineUserDpo user = new()
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = userName
        };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception();
        }

        return user;
    }
    
}

internal class UserAlreadyExistException : Exception
{
    public UserAlreadyExistException(string userName): base($"user {userName} already exists")
    {
     
    }
}