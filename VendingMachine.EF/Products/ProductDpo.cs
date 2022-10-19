using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VendingMachine.Domain;

namespace VendingMachine.EF.Products;

public class ProductDpo: IProduct
{
    
    public string Name { get; set; }
    
    public string SellerId { get; set; }
    public decimal Cost { get; set; }
    public int AmountAvailable { get; set; }
}