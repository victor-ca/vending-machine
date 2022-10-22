using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Auth;
using VendingMachine.Domain.Auth;
using VendingMachine.Domain.User;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace VendingMachine.EF.Users;

public class EfUserRepository : IUserRepository
{
    private readonly UserManager<VendingMachineUserDpo> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly TokenGeneratorConfig _config;
    private readonly VendingMachineDbContext _context;

    public EfUserRepository(
        UserManager<VendingMachineUserDpo> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenGenerator tokenGenerator,
        TokenGeneratorConfig config,
        VendingMachineDbContext context
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenGenerator = tokenGenerator;
        _config = config;
        _context = context;
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
        authClaims.Add(new Claim("userName", userName));
        bool isSeller = userRoles.FirstOrDefault(x => x == UserRoles.Seller) != null;
        authClaims.Add(new Claim("isSeller", isSeller ? "true" : "false"));

        var token = _tokenGenerator.CreateToken(authClaims);
        var refreshToken = _tokenGenerator.GenerateRefreshToken();

        _context.UserSessions.Add(new UserSession()
        {
            RefreshToken = refreshToken,
            UserName = userName,
            RefreshTokenExpiryTime = DateTime.Now.AddDays(_config.RefreshTokenValidityInDays)
        });

        await _userManager.UpdateAsync(user);
        await _context.SaveChangesAsync();

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

        var userSession = await _context.UserSessions
            .Where(x =>
                x.UserName == username
                && x.RefreshToken == refreshToken
                && x.RefreshTokenExpiryTime > DateTime.Now
            ).FirstOrDefaultAsync();

        if (user == null || userSession == null)
        {
            throw new Exception("Invalid access token or refresh token");
        }

        var newAccessToken = _tokenGenerator.CreateToken(principal.Claims.ToList());

        return new TokenCredentials
        {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = refreshToken,
            Expiration = newAccessToken.ValidTo
        };
    }

    public async Task<int> GetValidSessionsCount(string userName)
    {
        var now = DateTime.Now;
        var sessions = await _context.UserSessions.Where(x => x.UserName == userName && x.RefreshTokenExpiryTime > now)
            .ToListAsync();
        return sessions.Count;
    }

    public async Task DropOtherSessions(string userName, string keepRefreshToken)
    {
        var activeSessions = _context.UserSessions.Where(x =>
            x.UserName == userName
            && x.RefreshToken != keepRefreshToken);

        _context.UserSessions.RemoveRange(activeSessions);

        await _context.SaveChangesAsync();
    }

    public async Task DropSession(string userName, string dropToken)
    {
        var activeSession = await _context.UserSessions.FirstOrDefaultAsync(x =>
            x.UserName == userName
            && x.RefreshToken == dropToken);

        if (activeSession != null)
        {
            _context.UserSessions.Remove(activeSession);
            await _context.SaveChangesAsync();
        }
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
    public UserAlreadyExistException(string userName) : base($"user {userName} already exists")
    {
    }
}