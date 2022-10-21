namespace VendingMachine.Domain.Products;

public interface IProduct
{
    string Name { get; set; }
    decimal Cost { get; set; }
    int AmountAvailable { get; set; }
}
public class Product: IProduct
{
    public string Name { get; set; } = null!;
    public decimal Cost { get; set; }
    public int AmountAvailable { get; set; }
}