using VendingMachine.Domain.User;

namespace VendingMachine.Domain.Products;

public class OwnedOnlyProductService:IProductsService
{
    private readonly IProductsService _inner;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;

    public OwnedOnlyProductService(IProductsService inner, IProductRepository productRepository, ICurrentUserService currentUserService)
    {
        _inner = inner;
        _productRepository = productRepository;
        _currentUserService = currentUserService;
    }
    public Task<IEnumerable<IProduct>> GetAllAvailable()
    {
        return _inner.GetAllAvailable();
    }

    public Task<IEnumerable<IProduct>> GetAllOwned()
    {
        return _inner.GetAllOwned();
    }

    public Task<IProduct> Create(IProduct product)
    {
        return _inner.Create(product);
    }

    public async Task<IProduct> Delete(string productName)
    {
       await AssertUserOwnsTheProduct(productName);
       return await _inner.Delete(productName);

    }

    private async Task AssertUserOwnsTheProduct(string productName)
    {
        var ownerName = await _productRepository.GetProductOwnerByName(productName);
        var currentUserName = _currentUserService.GetCurrentUserName();
        if (_currentUserService.GetCurrentUserName() != ownerName)
        {
            throw new UnauthorizedAccessException($"{currentUserName} attempted to access a product that does not belong to him");
        }
    }

    public async Task<IProduct> UpdateProduct(string productName, IProduct details)
    {
        await AssertUserOwnsTheProduct(productName);
        return await _inner.UpdateProduct(productName, details);
    }
}