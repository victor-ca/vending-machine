namespace VendingMachine.Domain.CoinBank;

public static class CoinUtils
{
    public static readonly int[] AllowedDenominations = new[] { 100, 50, 20, 10, 5 };
    public static IEnumerable<int> CoinDictToCoinArraySortedDescending(Dictionary<int, int> coins)
    {
        var coinList = coins.SelectMany(keyValue => Enumerable.Range(0, keyValue.Value).Select(x => keyValue.Key))
            .ToList();

        coinList.Sort();
        coinList.Reverse();

        return coinList!;
    }

    public static Dictionary<int, int> CoinListToCoinDict(IEnumerable<int> coins)
    {
        return coins.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
    }
}