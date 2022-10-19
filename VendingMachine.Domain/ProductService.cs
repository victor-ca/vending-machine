namespace VendingMachine.Domain;

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
        var userId = _currentUserService.GetCurrentUserId();
        return _productRepository.GetOwnedByUser(userId);

    }

    public Task<Product> Create(Product product)
    {
        var userId = _currentUserService.GetCurrentUserId();
        return _productRepository.Create(userId,product);
    }

    public Task<Product> SetAmount(string productName, int amount)
    {
        var userId = _currentUserService.GetCurrentUserId();
        return _productRepository.SetProductAmount(userId, productName, amount);
    }

    public Task<Product> Delete(string productName)
    {
        var userId = _currentUserService.GetCurrentUserId();
        return _productRepository.DeleteProduct(userId, productName);
    }

}

