namespace VendingMachine.Domain.CoinBank;

public class PurchaseResult
{
    public int ChangeAmountInCents { get; set; }
    public int ActualSpentInCents { get; set; }
    public int PurchaseAmountInCents { get; set; }
    public Dictionary<int, int> UsedCoins { get; set; } = null!;
    public Dictionary<int, int> NewCoinBankState { get; set; } = null!;
    public Dictionary<int, int> ChangeCoins { get; set; } = null!;
}