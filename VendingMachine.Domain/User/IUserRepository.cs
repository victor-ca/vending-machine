namespace VendingMachine.Domain.User;

public interface IUserRepository
{
    
    Task CreateBuyer( string userName, string password);
    Task CreateSeller( string userName, string password);

    Task<ITokenCredentials> GetCredentials(string userName, string password);
    Task<ITokenCredentials> GetPrincipalFromExpiredToken(string accessToken, string refreshToken);

    Task<int> GetValidSessionsCount(string userName);
    Task DropOtherSessions(string userName, string keepRefreshToken);
    Task DropSession(string getCurrentUserName, string currentRefreshToken);
} 