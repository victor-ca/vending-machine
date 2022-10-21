using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Domain;
using VendingMachine.Domain.Products;
using VendingMachine.Domain.User;

namespace VendingMachine.API.Controllers;

[ApiController]

[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;


    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet("for-sale")]
    [Authorize(Roles = UserRoles.Buyer)]
    public Task<IEnumerable<Product>> GetAll()
    {
        return _productsService.GetAllAvailable();
    }
    
    [HttpGet("owned")]
    [Authorize(Roles = UserRoles.Seller)]
    public Task<IEnumerable<Product>> GetOwned()
    {
        return _productsService.GetAllOwned();
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoles.Seller)]
    public Task<Product> Create([FromBody]Product product)
    {
        return _productsService.Create(product);
    }
    
    [HttpPut("{productName}/amount")]
    [Authorize(Roles = UserRoles.Seller)]
    public Task<Product> Create(string productName, [FromBody]int amount)
    {
        return _productsService.SetAmount(productName,amount);
    }
    
    [HttpDelete("{productName}")]
    [Authorize(Roles = UserRoles.Seller)]
    public Task<Product> Delete(string productName)
    {
        return _productsService.Delete(productName);
    }
}

