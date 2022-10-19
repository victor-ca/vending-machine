namespace VendingMachine.Domain;

public interface IVendingMachineUser
{
    string UserName { get; set; }
    bool IsVendor { get; set; }
}
public class VendingMachineUser:IVendingMachineUser
{
    public string UserName { get; set; }
    public bool IsVendor { get; set; }
}