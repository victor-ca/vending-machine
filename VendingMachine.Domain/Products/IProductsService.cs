namespace VendingMachine.Domain.Products;

public interface IProductsService
{
    Task<IEnumerable<Product>> GetAllAvailable();
    Task<IEnumerable<Product>> GetAllOwned();
    Task<Product> Create(Product product);
    Task<Product> SetAmount(string productName, int amount);
    Task<Product> Delete(string productName);
}