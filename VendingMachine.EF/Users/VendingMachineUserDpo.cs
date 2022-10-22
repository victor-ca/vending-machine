using Microsoft.AspNetCore.Identity;
using VendingMachine.Domain.Products;

namespace VendingMachine.EF.Users;

public class VendingMachineUserDpo: IdentityUser, IVendingMachineUser
{
 
}