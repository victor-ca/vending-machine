namespace VendingMachine.Auth;

#pragma warning disable CS8618
public class TokenGeneratorConfig
{
    public string Secret { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public int  TokenValidityInMinutes { get; init; }
    public int RefreshTokenValidityInDays { get; init; }
}
#pragma warning restore CS8618
