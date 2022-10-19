namespace VendingMachine.Domain;

public interface IProduct
{
    string Name { get; set; }
    decimal Cost { get; set; }
    int AmountAvailable { get; set; }
}
public class Product: IProduct
{
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public int AmountAvailable { get; set; }
}