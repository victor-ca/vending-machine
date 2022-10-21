using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Auth;

namespace VendingMachine.API.Controllers;

[Authorize]
[ApiController]
[Route("api/auth")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;

    public UsersController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] LoginModel loginModel)
    {
        var authResult = await _authService.Authenticate(loginModel);

        return Ok(authResult);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshToken)
    {
        var authResult = await _authService.RefreshToken(refreshToken);

        return Ok(authResult);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var authResult = await _authService.Register(registerModel);
        return Ok(authResult);
    }
}