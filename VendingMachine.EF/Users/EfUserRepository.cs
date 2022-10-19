using Microsoft.EntityFrameworkCore;
using VendingMachine.Domain;
using VendingMachine.Domain.User;

namespace VendingMachine.EF.Users;

public class EfUserRepository: IUserRepository
{
    private readonly VendingMachineDbContext _dbContext;

    public EfUserRepository(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task<VendingMachineUserDpo> GetDpoUserByName(string userName)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
        {
            throw new UserNotFoundException(userName);
        }

        return user;
    } 
    public async Task<VendingMachineUser> GetByUserName(string userName)
    {
        var user = await GetDpoUserByName(userName);

        return new VendingMachineUser
        {
            IsVendor = user.IsVendor,
            UserName = userName
        };
    }

    public async Task<VendingMachineUserCredentials> GetCredentialsByUserName(string userName)
    {
        var user = await GetDpoUserByName(userName);
        return new VendingMachineUserCredentials()
        {
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt
        };
    }

    public async Task<VendingMachineUser> CreateUser(VendingMachineUser user, VendingMachineUserCredentials credentials)
    {
        _dbContext.Users.Add(new VendingMachineUserDpo
        {
            UserName = user.UserName,
            IsVendor = user.IsVendor,
            PasswordHash = credentials.PasswordHash,
            PasswordSalt = credentials.PasswordSalt,
        });
        await _dbContext.SaveChangesAsync();
        return await GetByUserName(user.UserName);
    }
}

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userName): base($"and user named [{userName}] was not found")
    {
        
    }
}