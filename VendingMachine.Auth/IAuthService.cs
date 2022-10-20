#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using VendingMachine.Domain.User;

namespace VendingMachine.Auth;

public interface IAuthService
{
    Task<ITokenCredentials> Authenticate(LoginModel loginModel);
    Task<ITokenCredentials> Register(RegisterModel registerModel);
    Task<ITokenCredentials> RefreshToken(RefreshTokenRequest refreshTokenRequest);
}


public class RegisterModel : LoginModel
{
    public bool IsSeller { get; set; }
}

public class LoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}

public class RefreshTokenRequest
{

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
#pragma warning restore CS8618