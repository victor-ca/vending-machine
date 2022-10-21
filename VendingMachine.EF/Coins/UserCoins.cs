namespace VendingMachine.EF.Coins;

public  class UserCoins
{
    public int CentDenominator { get; set; }
    public int Amount { get; set; }
    public string UserName { get; set; } = null!;
}