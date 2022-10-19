namespace VendingMachine.Domain.User;

public interface IUserCredentials
{
     byte[] PasswordHash { get; set; }
     byte[] PasswordSalt { get; set; }
}

public class VendingMachineUserCredentials : IUserCredentials
{
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}