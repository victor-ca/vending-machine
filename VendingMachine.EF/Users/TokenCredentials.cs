using VendingMachine.Domain.User;

namespace VendingMachine.EF.Users;

#pragma warning disable CS8618
public class TokenCredentials : ITokenCredentials
{
    public string Token { get; init; }
    public string RefreshToken { get; init; }
    public DateTime Expiration { get; init; }
}
#pragma warning restore CS8618
