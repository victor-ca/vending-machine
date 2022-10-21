namespace VendingMachine.Domain.User;


public interface ITokenCredentials
{
    string Token { get;  }
    string RefreshToken { get;  }
    DateTime Expiration { get;  } 
}


public interface IRefreshTokenCredentials
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}