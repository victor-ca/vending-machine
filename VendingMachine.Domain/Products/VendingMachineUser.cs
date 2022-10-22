namespace VendingMachine.Domain.Products;

public interface IVendingMachineUser
{
    string UserName { get; set; }
    
}
public class VendingMachineUser: IVendingMachineUser
{
    public string UserName { get; set; } = null!;
}