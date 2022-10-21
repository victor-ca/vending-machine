namespace VendingMachine.Domain.User;

public interface ICurrentUserService
{
    string GetCurrentUserName();
}