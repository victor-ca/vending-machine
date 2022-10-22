using Microsoft.EntityFrameworkCore;
using VendingMachine.Domain.Products;

namespace VendingMachine.EF.Products;

public class EfProductRepository : IProductRepository
{
    private readonly VendingMachineDbContext _context;

    public EfProductRepository(VendingMachineDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<IProduct>> GetAllAvailable()
    {
        var products = await _context.Products.Where(x => x.AmountAvailable > 0).ToListAsync();
        return ToDomain(products);
    }


    public async Task<IEnumerable<IProduct>> GetOwnedByUser(string userName)
    {
        var products = await _context.Products.Where(x => x.SellerId == userName).ToListAsync();
        return ToDomain(products);
    }

    public async Task<IProduct> Create(string userId, IProduct product)
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

    public async Task<IProduct> SetProductAmount(string productName, int amountAvailable)
    {
        ProductDpo p = await GetProductByDpoByName(productName);

        p.AmountAvailable = amountAvailable;

        await _context.SaveChangesAsync();
        return ToDomain(p);
    }

    public async Task<IProduct> UpdateProduct(string productName, IProduct update)
    {
        ProductDpo p = await GetProductByDpoByName(productName);
        if (update.Name != productName)
        {
            _context.Products.Remove(p);
            var newProduct = new ProductDpo()
            {
                Name = update.Name,
                AmountAvailable = update.AmountAvailable,
                Cost = update.Cost,
                SellerId = p.SellerId
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return ToDomain(newProduct);
        }

        p.Cost = update.Cost;
        p.AmountAvailable = update.AmountAvailable;

        await _context.SaveChangesAsync();
        return ToDomain(p);
    }

    private async Task<ProductDpo> GetProductByDpoByName(string productName)
    {
        var p = await _context.Products
            .Where(x => x.Name == productName)
            .FirstOrDefaultAsync();

        if (p == null)
        {
            throw new ProductNotFoundException(productName);
        }

        return p;
    }

    public async Task<IProduct> DeleteProduct(string userId, string productName)
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

    public async Task<IProduct> GetProductByName(string productName)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Name == productName);
        if (product == null)
        {
            throw new ProductNotFoundException(productName);
        }

        return ToDomain(product);
    }


    private IEnumerable<IProduct> ToDomain(List<ProductDpo> products)
    {
        return products.Select(ToDomain);
    }

    private IProduct ToDomain(ProductDpo p)
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