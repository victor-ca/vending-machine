using Microsoft.AspNetCore.Identity;
using VendingMachine.Domain.Products;
using VendingMachine.Domain.User;

namespace VendingMachine.EF.Users;

public class VendingMachineUserDpo: IdentityUser, IVendingMachineUser,IRefreshTokenCredentials
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    
}