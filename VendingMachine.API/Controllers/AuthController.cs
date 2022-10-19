using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.API.DTOs;
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
    public async Task<IActionResult> Authenticate([FromBody] UserDto userDto)
    {
        var authResult = await _authService.Authenticate(userDto.Username, userDto.Password);

        return Ok(authResult);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        try
        {
            var authResult = await _authService.CreateUser(userDto.Username, userDto.Password);
            return Ok(authResult);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}