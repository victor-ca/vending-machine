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

    public async Task<Dictionary<int, int>> PurchaseProduct(PurchaseRequest request)
    {
        var coins = await GetAvailableCoins();
        var product = await _productRepository.GetProductByName(request.ProductName);
        var coinsWithChange =  AttemptPurchase(product, request.DesiredAmount, coins);
        await _productRepository.SetProductAmount(product.Name, product.AmountAvailable -= request.DesiredAmount);
        var userName = _currentUserService.GetCurrentUserName();
        await _coinBankRepo.ResetUserCoinsTo(userName,coinsWithChange);

        return coinsWithChange;
    }

    private Dictionary<int, int> AttemptPurchase(Product product, int amountToPurchase,
        Dictionary<int, int> coins)
    {
        var unpaidAmountInCents = product.Cost * amountToPurchase * 100;
        var remainingCoins = CoinUtils.CoinDictToCoinArraySortedDescending(coins).ToList();

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

            remainingCoins.Remove(coinToSpend);

            unpaidAmountInCents -= coinToSpend;
        }

        
        while (unpaidAmountInCents < 0)
        {
            var changeCoin = CoinUtils.AllowedDenominations.First(x => unpaidAmountInCents + x <= 0);
            unpaidAmountInCents += changeCoin;
            remainingCoins.Add(changeCoin);
        }

        return CoinUtils.CoinListToCoinDict(remainingCoins);
    }

    public async Task Reset()
    {
        var user = _currentUserService.GetCurrentUserName();
        await _coinBankRepo.ClearUserCoins(user);
    }
}

internal class NotEnoughCoinsException : Exception
{
}