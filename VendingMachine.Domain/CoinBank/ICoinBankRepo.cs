namespace VendingMachine.Domain.CoinBank;

public interface ICoinBankRepo
{
    Task InsertCoin(string userName, int centDenominator);
    Task<Dictionary<int,int>> GetAvailableCoins(string userName);
    Task ClearUserCoins(string userName);
    Task ResetUserCoinsTo(string userName, Dictionary<int, int> coinBank);
}