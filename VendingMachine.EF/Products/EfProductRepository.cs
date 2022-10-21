using Microsoft.EntityFrameworkCore;
using VendingMachine.Domain;
using VendingMachine.Domain.Products;

namespace VendingMachine.EF.Products;

public class EfProductRepository : IProductRepository
{
    private readonly VendingMachineDbContext _context;

    public EfProductRepository(VendingMachineDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAvailable()
    {
        var products = await _context.Products.Where(x => x.AmountAvailable > 0).ToListAsync();
        return ToDomain(products);
    }


    public async Task<IEnumerable<Product>> GetOwnedByUser(string userName)
    {
        var products = await _context.Products.Where(x => x.SellerId == userName).ToListAsync();
        return ToDomain(products);
    }

    public async Task<Product> Create(string userId, Product product)
    {
        var p = new ProductDpo
        {
            Cost = product.Cost,
            Name = product.Name,
            AmountAvailable = product.AmountAvailable,
            SellerId = userId
        };

        _context.Products.Add(p);
        await _context.SaveChangesAsync();
        return ToDomain(p);
    }

    public async Task<Product> SetProductAmount(string productName, int amount)
    {
        var p = await _context.Products
            .Where(x => x.Name == productName)
            .FirstOrDefaultAsync();
        
        if (p == null)
        {
            throw new ProductNotFoundException(productName);
        }

        p.AmountAvailable = amount;
        await _context.SaveChangesAsync();
        return ToDomain(p);
    }

    public async Task<Product> DeleteProduct(string userId, string productName)
    {
        var product = await _context.Products.Where(x => x.SellerId == userId && x.Name == productName)
            .FirstOrDefaultAsync();

        if (product == null)
        {
            throw new ProductNotFoundException(productName);
        }

        _context.Remove(product);
        await _context.SaveChangesAsync();

        return ToDomain(product);
    }

    public async Task<Product> GetProductByName(string productName)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == productName);
        if (product == null)
        {
            throw new ProductNotFoundException(productName);
        }

        return ToDomain(product);

    }


    private IEnumerable<Product> ToDomain(List<ProductDpo> products)
    {
        return products.Select(ToDomain);
    }

    private Product ToDomain(ProductDpo p)
    {
        return new Product { Cost = p.Cost, Name = p.Name, AmountAvailable = p.AmountAvailable };
    }
}

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string productName) : base($"product ${productName} was not found")
    {
    }
}