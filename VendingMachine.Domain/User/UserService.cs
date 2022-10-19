namespace VendingMachine.Domain.User;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task GetByUserName(string userName)
    {
        return _userRepository.GetByUserName(userName);
    }

    Task<VendingMachineUser> IUserService.GetByUserName(string userName)
    {
        return _userRepository.GetByUserName(userName);
    }

    public Task<VendingMachineUserCredentials> GetCredentialsByUsername(string userName)
    {
      return  _userRepository.GetCredentialsByUserName(userName);
    }

    public Task<VendingMachineUser> CreateUser(VendingMachineUser user, VendingMachineUserCredentials credentials)
    {
        return _userRepository.CreateUser(user, credentials);
    }
}