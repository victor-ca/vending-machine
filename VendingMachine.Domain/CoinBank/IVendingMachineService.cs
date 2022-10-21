namespace VendingMachine.Domain.CoinBank;

public interface IVendingMachineService
{
    Task InsertCoin(int centDenominator);
    Task<Dictionary<int, int>> GetAvailableCoins();

    Task<Dictionary<int, int>> PurchaseProduct(PurchaseRequest request);
    Task Reset();
}