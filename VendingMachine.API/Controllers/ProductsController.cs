using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public Task<IEnumerable<IProduct>> GetAll()
    {
        return _productsService.GetAllAvailable();
    }
    
    [HttpGet("owned")]
    [Authorize(Roles = UserRoles.Seller)]
    public Task<IEnumerable<IProduct>> GetOwned()
    {
        return _productsService.GetAllOwned();
    }
    
    [HttpPost]
    [Authorize(Roles = UserRoles.Seller)]
    public Task<IProduct> Create([FromBody]Product product)
    {
        return _productsService.Create(product);
    }
    
    [HttpPut("{productName}")]
    [Authorize(Roles = UserRoles.Seller)]
    public Task<IProduct> Create(string productName, [FromBody]Product details)
    {
        return _productsService.UpdateProduct(productName, details);
    }
    
    [HttpDelete("{productName}")]
    [Authorize(Roles = UserRoles.Seller)]
    public Task<IProduct> Delete(string productName)
    {
        return _productsService.Delete(productName);
    }
}

