using VendingMachine.Domain;
using VendingMachine.Domain.Products;

namespace VendingMachine.EF.Products;

#pragma warning disable CS8618
public class ProductDpo: IProduct
{
    
    public string Name { get; set; }
    
    public decimal Cost { get; set; }
    public int AmountAvailable { get; set; }
    
    public string SellerId { get; set;}
}
#pragma warning restore CS8618

