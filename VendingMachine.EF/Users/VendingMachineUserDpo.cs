using VendingMachine.Domain;
using VendingMachine.Domain.User;

namespace VendingMachine.EF.Users;

public class VendingMachineUserDpo: IVendingMachineUser, IUserCredentials
{
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string UserName { get; set; }
    public bool IsVendor { get; set; }
}