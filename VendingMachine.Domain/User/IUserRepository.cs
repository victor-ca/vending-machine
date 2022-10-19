namespace VendingMachine.Domain.User;

public interface IUserRepository
{
    Task<VendingMachineUser> GetByUserName(string userName);
    Task<VendingMachineUserCredentials> GetCredentialsByUserName(string userName);
    Task<VendingMachineUser> CreateUser(VendingMachineUser user, VendingMachineUserCredentials credentials);
}