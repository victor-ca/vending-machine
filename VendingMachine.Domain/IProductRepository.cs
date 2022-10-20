namespace VendingMachine.Domain;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAvailable();
    Task<IEnumerable<Product>> GetOwnedByUser(string userName);
    Task<Product> Create(string userId, Product product);
    Task<Product> SetProductAmount(string userId, string productName, int amount);
    Task<Product> DeleteProduct(string userId, string productName);
}