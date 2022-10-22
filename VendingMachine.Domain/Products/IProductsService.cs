namespace VendingMachine.Domain.Products;

public interface IProductsService
{
    Task<IEnumerable<IProduct>> GetAllAvailable();
    Task<IEnumerable<IProduct>> GetAllOwned();
    Task<IProduct> Create(IProduct product);
    
    Task<IProduct> Delete(string productName);
    Task<IProduct> UpdateProduct(string productName, IProduct details);
}