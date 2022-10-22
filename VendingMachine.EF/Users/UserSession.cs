namespace VendingMachine.EF.Users;

public class UserSession
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string UserName { get; set; } 
}