using VendingMachine.Domain.User;

namespace VendingMachine.Domain.Products;

public class ProductService : IProductsService
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;

    public ProductService(IProductRepository productRepository,ICurrentUserService currentUserService)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
    }

    public Task<IEnumerable<Product>> GetAllAvailable()
    {
        return _productRepository.GetAllAvailable();
    }

    public Task<IEnumerable<Product>> GetAllOwned()
    {
        var userName = _currentUserService.GetCurrentUserName();
        return _productRepository.GetOwnedByUser(userName);

    }

    public Task<Product> Create(Product product)
    {
        var userName = _currentUserService.GetCurrentUserName();
        return _productRepository.Create(userName,product);
    }

    public Task<Product> SetAmount(string productName, int amount)
    {
        return _productRepository.SetProductAmount(productName, amount);
    }

    public Task<Product> Delete(string productName)
    {
        var userId = _currentUserService.GetCurrentUserName();
        return _productRepository.DeleteProduct(userId, productName);
    }

}

