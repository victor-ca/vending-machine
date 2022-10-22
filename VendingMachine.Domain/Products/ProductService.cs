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

    public Task<IEnumerable<IProduct>> GetAllAvailable()
    {
        return _productRepository.GetAllAvailable();
    }

    public Task<IEnumerable<IProduct>> GetAllOwned()
    {
        var userName = _currentUserService.GetCurrentUserName();
        return _productRepository.GetOwnedByUser(userName);

    }

    public Task<IProduct> Create(IProduct product)
    {
        var userName = _currentUserService.GetCurrentUserName();
        return _productRepository.Create(userName,product);
    }


    public Task<IProduct> Delete(string productName)
    {
        var userId = _currentUserService.GetCurrentUserName();
        return _productRepository.DeleteProduct(userId, productName);
    }

    public Task<IProduct> UpdateProduct(string productName, IProduct details)
    {
        return _productRepository.UpdateProduct(productName, details);
    }
}

