using VendingMachine.Domain.Products;
using VendingMachine.Domain.User;

namespace VendingMachine.Domain.CoinBank;

public class VendingMachineService : IVendingMachineService
{
    private readonly ICoinBankRepo _coinBankRepo;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProductRepository _productRepository;

    public VendingMachineService(ICoinBankRepo coinBankRepo, ICurrentUserService currentUserService,
        IProductRepository productRepository)
    {
        _coinBankRepo = coinBankRepo;
        _currentUserService = currentUserService;
        _productRepository = productRepository;
    }

    public async Task InsertCoin(int centDenominator)
    {
        var user = _currentUserService.GetCurrentUserName();
        await _coinBankRepo.InsertCoin(user, centDenominator);
    }

    public async Task<Dictionary<int, int>> GetAvailableCoins()
    {
        var userName = _currentUserService.GetCurrentUserName();
        return await _coinBankRepo.GetAvailableCoins(userName);
    }

    public async Task<PurchaseResult> PurchaseProduct(PurchaseRequest request)
    {
        var coins = await GetAvailableCoins();
        var product = await _productRepository.GetProductByName(request.ProductName);
        var purchaseResult =  AttemptPurchase(product, request.DesiredAmount, coins);
        await _productRepository.SetProductAmount(product.Name, product.AmountAvailable -= request.DesiredAmount);
        var userName = _currentUserService.GetCurrentUserName();
        await _coinBankRepo.ResetUserCoinsTo(userName,purchaseResult.NewCoinBankState);

        return purchaseResult;
    }

    private PurchaseResult AttemptPurchase(IProduct product, int amountToPurchase,
        Dictionary<int, int> coins)
    {
        var totalCostInCents =  product.Cost * amountToPurchase * 100;
        var actualSpentInCents = 0;
        var unpaidAmountInCents = totalCostInCents;
        var remainingCoins = CoinUtils.CoinDictToCoinArraySortedDescending(coins).ToList();
        var usedCoins = new List<int>();

        while (unpaidAmountInCents > 0)
        {
            if (remainingCoins.Count == 0)
            {
                throw new NotEnoughCoinsException();
            }

            var coinToSpend = remainingCoins.FirstOrDefault(coin => coin < unpaidAmountInCents);
            if (coinToSpend == 0)
            {
                coinToSpend = remainingCoins.First();
            }
            usedCoins.Add(coinToSpend);
            remainingCoins.Remove(coinToSpend);

            unpaidAmountInCents -= coinToSpend;
            actualSpentInCents += coinToSpend;
        }

        PurchaseResult result = new PurchaseResult
        {
            ActualSpentInCents = actualSpentInCents,
            PurchaseAmountInCents = (int)totalCostInCents,
            ChangeAmountInCents = (int)Math.Abs(unpaidAmountInCents),
            UsedCoins =  CoinUtils.CoinListToCoinDict(usedCoins)
        };
        
        var changeCoins = new List<int>();
        while (unpaidAmountInCents < 0)
        {
            var changeCoin = CoinUtils.AllowedDenominations.FirstOrDefault(x => unpaidAmountInCents + x <= 0);
            if (changeCoin == 0)
            {
                throw new ChangeAmountCannotBeFormedUsingSpecifiedDenominatorException(result.ChangeAmountInCents);
            }

            unpaidAmountInCents += changeCoin;
            changeCoins.Add(changeCoin);
        }

        remainingCoins.AddRange(changeCoins);
        result.NewCoinBankState = CoinUtils.CoinListToCoinDict(remainingCoins);
        result.ChangeCoins = CoinUtils.CoinListToCoinDict(changeCoins);

        return result;
    }

    public async Task Reset()
    {
        var user = _currentUserService.GetCurrentUserName();
        await _coinBankRepo.ClearUserCoins(user);
    }
}

internal class ChangeAmountCannotBeFormedUsingSpecifiedDenominatorException : Exception
{
    public ChangeAmountCannotBeFormedUsingSpecifiedDenominatorException(decimal change):base($"a change of {change} cannot be returned using allowed denominators")
    {
        
    }
}

internal class NotEnoughCoinsException : Exception
{
}