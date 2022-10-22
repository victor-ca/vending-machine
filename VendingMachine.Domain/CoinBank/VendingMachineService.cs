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
        var product = await _productRepository.GetProductByName(request.ProductName);
        if (request.DesiredAmount > product.AmountAvailable)
        {
            throw new ProductAmountUnavailableException(request, product.AmountAvailable);
        }

        var coins = await GetAvailableCoins();
        var purchaseResult = AttemptPurchase(product, request.DesiredAmount, coins);
        await _productRepository.SetProductAmount(product.Name, product.AmountAvailable -= request.DesiredAmount);
        var userName = _currentUserService.GetCurrentUserName();
        await _coinBankRepo.ResetUserCoinsTo(userName, purchaseResult.NewCoinBankState);

        return purchaseResult;
    }

    private PurchaseResult AttemptPurchase(IProduct product, int amountToPurchase,
        Dictionary<int, int> coins)
    {
        var totalCostInCents = product.Cost * amountToPurchase * 100;
        var actualSpentInCents = 0;
        var unpaidAmountInCents = totalCostInCents;
        var remainingCoins = CoinUtils.CoinDictToCoinArraySortedDescending(coins).ToList();
        var usedCoins = new List<int>();

        while (unpaidAmountInCents > 0)
        {
            if (remainingCoins.Count == 0)
            {
                throw new NotEnoughCoinsException((int)unpaidAmountInCents);
            }

            var spendCandidates = remainingCoins.Where(coin => coin <= unpaidAmountInCents).ToList();


            var coinToSpend = spendCandidates.Count == 0 ? remainingCoins.First() : spendCandidates.Max();

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
            UsedCoins = CoinUtils.CoinListToCoinDict(usedCoins)
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

public class ProductAmountUnavailableException : Exception
{
    public ProductAmountUnavailableException(PurchaseRequest request, int availableAmount) : base(
        $" only {availableAmount} of the {request.DesiredAmount} items of {request.ProductName} are available")
    {
    }
}

internal class ChangeAmountCannotBeFormedUsingSpecifiedDenominatorException : Exception
{
    public ChangeAmountCannotBeFormedUsingSpecifiedDenominatorException(decimal change) : base(
        $"The change of {change} cents cannot be returned using allowed denominations")
    {
    }
}

internal class NotEnoughCoinsException : Exception
{
    public NotEnoughCoinsException(int howMany) : base(
        $"{howMany} more cents are required to complete this transaction")
    {
    }
}