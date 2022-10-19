namespace VendingMachine.Auth;

public interface IAuthService
{
    Task<AuthenticateResult> Authenticate(string name, string password);
    Task<AuthenticateResult> CreateUser(string userName, string password);

   
}