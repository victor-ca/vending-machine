namespace VendingMachine.Domain.User;

public interface IUserService
{
    Task<VendingMachineUser> GetByUserName(string userName);
    Task<VendingMachineUserCredentials> GetCredentialsByUsername(string username);
    Task<VendingMachineUser> CreateUser(VendingMachineUser user, VendingMachineUserCredentials credentials);
}