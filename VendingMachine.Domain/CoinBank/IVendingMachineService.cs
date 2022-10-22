namespace VendingMachine.Domain.CoinBank;

public interface IVendingMachineService
{
    Task InsertCoin(int centDenominator);
    Task<Dictionary<int, int>> GetAvailableCoins();

    Task<PurchaseResult> PurchaseProduct(PurchaseRequest request);
    Task Reset();
}