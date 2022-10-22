using Microsoft.AspNetCore.Mvc;
using VendingMachine.Domain.User;

namespace VendingMachine.API.Controllers;

[ApiController]
[Route("api")]
public class BonusController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public BonusController(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    [HttpGet("active-sessions")]
    public async Task<IActionResult> CountActiveSessions()
    {
        var authResult = await _userRepository.GetValidSessionsCount(_currentUserService.GetCurrentUserName());
        return Ok(authResult);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogOutAll([FromBody] string currentRefreshToken)
    {
        await _userRepository.DropSession(_currentUserService.GetCurrentUserName(), currentRefreshToken);
        return Accepted();
    }

    [HttpPost("logout/all")]
    public async Task<IActionResult> LogOutAllExceptCurrent([FromBody] string keepRefreshToken)
    {
        await _userRepository.DropOtherSessions(_currentUserService.GetCurrentUserName(), keepRefreshToken);
        return Accepted();
    }
}