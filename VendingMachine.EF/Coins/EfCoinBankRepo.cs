using Microsoft.EntityFrameworkCore;
using VendingMachine.Domain.CoinBank;

namespace VendingMachine.EF.Coins;

public class EfCoinBankRepo: ICoinBankRepo
{
    private readonly VendingMachineDbContext _context;

    public EfCoinBankRepo(VendingMachineDbContext context)
    {
        _context = context;
    }
    public async  Task InsertCoin(string userName, int centDenominator)
    {
        var coin =await _context.CoinBank.Where(c => c.CentDenominator == centDenominator && c.UserName == userName).FirstOrDefaultAsync();
        if (coin == null)
        {
            _context.CoinBank.Add(
                new UserCoins() { Amount = 1, CentDenominator = centDenominator, UserName = userName });
        }
        else
        {
            coin.Amount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<int,int>> GetAvailableCoins(string userName)
    {
        var coins = await _context.CoinBank.Where(c =>c.UserName == userName).ToListAsync();
        var dict = coins.ToDictionary(c =>  c.CentDenominator, c=> c.Amount);
        return dict;
    }

    public async Task ClearUserCoins(string userName)
    {
        _context.CoinBank.RemoveRange(_context.CoinBank.Where(c => c.UserName == userName));
        await _context.SaveChangesAsync();
    }

    public async Task ResetUserCoinsTo(string userName, Dictionary<int, int> coinBank)
    {
        foreach (var denomination in CoinUtils.AllowedDenominations)
        {
            var coin = await _context.CoinBank.Where(c => c.CentDenominator == denomination && c.UserName == userName).FirstOrDefaultAsync();
            if (coin == null)
            {
                _context.CoinBank.Add(
                    new UserCoins() { Amount = coinBank[denomination], CentDenominator = denomination, UserName = userName });
            }
            else
            {
                coin.Amount = coinBank[denomination];
            }

        }

        await _context.SaveChangesAsync();
    }
}