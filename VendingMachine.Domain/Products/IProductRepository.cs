namespace VendingMachine.Domain.Products;

public interface IProductRepository
{
    Task<IEnumerable<IProduct>> GetAllAvailable();
    Task<IEnumerable<IProduct>> GetOwnedByUser(string userName);
    Task<IProduct> Create(string userId, IProduct product);
    Task<IProduct> UpdateProduct(string productName, IProduct update);
    Task<IProduct> DeleteProduct(string userId, string productName);
    Task<IProduct> GetProductByName(string productName);
    Task<IProduct> SetProductAmount(string productName, int productAmountAvailable);
}