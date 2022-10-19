using Microsoft.AspNetCore.Mvc;
using VendingMachine.Domain;

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
    public Task<IEnumerable<Product>> GetAll()
    {
        return _productsService.GetAllAvailable();
    }
    
    [HttpGet("owned")]
    public Task<IEnumerable<Product>> GetOwned()
    {
        return _productsService.GetAllOwned();
    }
    
    [HttpPost]
    public Task<Product> Create([FromBody]Product product)
    {
        return _productsService.Create(product);
    }
    
    [HttpPut("{productName}/amount")]
    public Task<Product> Create(string productName, [FromBody]int amount)
    {
        return _productsService.SetAmount(productName,amount);
    }
    [HttpDelete("{productName}")]
    public Task<Product> Delete(string productName)
    {
        return _productsService.Delete(productName);
    }
}

