using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Domain.CoinBank;
using VendingMachine.Domain.User;

namespace VendingMachine.API.Controllers;

[ApiController]
[Route("api")]
public class VendingMachineController : ControllerBase
{
    private readonly IVendingMachineService _vendingMachineService;

    public VendingMachineController(IVendingMachineService vendingMachineService)
    {
        _vendingMachineService = vendingMachineService;
    }


    [HttpPost("buy")]
    [Authorize(Roles = UserRoles.Buyer)]
    public async Task<IActionResult> PurchaseProduct(PurchaseRequest request)
    {
        await _vendingMachineService.PurchaseProduct(request);
        return Accepted();
    }

    
    [HttpPost("deposit")]
    [Authorize(Roles = UserRoles.Buyer)]
    public async Task<IActionResult> InsertCoin([FromBody] int centsDenominator)
    {
        await _vendingMachineService.InsertCoin(centsDenominator);
        return Accepted();
    }

    [HttpGet("coins")]
    [Authorize(Roles = UserRoles.Buyer)]
    public async Task<IActionResult> GetCoins()
    {
        var coins = await _vendingMachineService.GetAvailableCoins();
        return Ok(coins);
    }

    [HttpPost("reset")]
    [Authorize(Roles = UserRoles.Buyer)]
    public async Task<IActionResult> ResetCoins()
    {
        await _vendingMachineService.Reset();
        return Accepted();
    }
}