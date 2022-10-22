using FluentAssertions;
using NSubstitute;
using VendingMachine.Domain.CoinBank;
using VendingMachine.Domain.Products;
using VendingMachine.Domain.User;
using Xbehave;

namespace VendingMachine.Domain.Tests;

public class VendingMachineServiceTests
{
    private readonly IVendingMachineService _vendingMachineService;
    readonly ICoinBankRepo _coinBankRepo;
    readonly IProductRepository _productRepo;

    public VendingMachineServiceTests()
    {
        _productRepo = Substitute.For<IProductRepository>();
        _coinBankRepo = Substitute.For<ICoinBankRepo>();
        var currentUserService = Substitute.For<ICurrentUserService>();

        _vendingMachineService = new VendingMachineService(
            _coinBankRepo, currentUserService, _productRepo
        );
    }

    [Scenario]
    [Example(3, 3, true)]
    [Example(3, 1, true)]
    [Example(1, 3, false)]
    public void RequireEnoughProducts(int availableAmount, int desiredAmount, bool exceptSuccess)
    {
        $"Given {availableAmount} products are available".x(() =>
        {
            _productRepo.GetProductByName(string.Empty).ReturnsForAnyArgs(new Product()
                { AmountAvailable = availableAmount, Cost = 10 });
        });


        Func<Task>? purchaseFn = null;
        $"When attempting to purchase {desiredAmount} with enough funds".x(() =>
        {
            _coinBankRepo.GetAvailableCoins(String.Empty).ReturnsForAnyArgs(new Dictionary<int, int> { { 100, 100 } });
            purchaseFn = () => _vendingMachineService.PurchaseProduct(new PurchaseRequest()
                { DesiredAmount = desiredAmount });
        });

        if (exceptSuccess)
        {
            "The attempt should succeed".x(async () => { await purchaseFn.Should().NotThrowAsync(); });
        }
        else
        {
            "The attempt should fail with an explicit exception".x(async () =>
            {
                await purchaseFn.Should().ThrowAsync<ProductAmountUnavailableException>();
            });
        }
    }

    [Scenario]
    [Example("2x100", "4x0.5", 0)]
    [Example("3x100", "5x0.5", 50)]
    [Example("1x50", "5x0.03", 35)]
    [Example("1x20", "3x0.05", 5)]
    [Example("1x10|2x5|1x50", "1x0.65", 0)]
    public void BuySuccess(string centsBank, string purchase, int centsChange)
    {
        var splitPurchase = purchase.Split("x");
        var buyAmount = int.Parse(splitPurchase[0]);
        var productPriceInUsd = decimal.Parse(splitPurchase[1]);
        var coins = new Dictionary<int, int>();
        foreach (var coinRaw in centsBank.Split("|"))
        {
            var coinAndAmount = coinRaw.Split("x").Select(int.Parse).ToArray();
            coins.Add(coinAndAmount[1], coinAndAmount[0]);
        }

        $"Given {centsBank} coins available".x(() =>
        {
            _coinBankRepo.GetAvailableCoins(String.Empty).ReturnsForAnyArgs(coins);
        });


        Func<Task<PurchaseResult>>? purchaseFn = null;
        $"When attempting to purchase {buyAmount} products at ${productPriceInUsd} per item (unlimited stock)".x(() =>
        {
            _productRepo.GetProductByName(string.Empty).ReturnsForAnyArgs(new Product()
            {
                Cost = productPriceInUsd,
                AmountAvailable = Int32.MaxValue,
            });
            purchaseFn = () => _vendingMachineService.PurchaseProduct(new PurchaseRequest()
                { DesiredAmount = buyAmount });
        });


        $"The attempt should succeed with a change of ${centsChange}".x(async () =>
        {
            var res = await purchaseFn!();
            res.ChangeAmountInCents.Should().Be(centsChange);
        });
    }

    [Scenario]
    [Example("1x100", "4x0.5", "100 more cents are required to complete this transaction")]
    [Example("1x5", "4x0.01", "The change of 1 cents cannot be returned using allowed denominations")]
    [Example("1x5", "7x0.01", "2 more cents are required to complete this transaction")]
    public void BuyFailure(string centsBank, string purchase, string expectedErrorMessage)
    {
        var splitPurchase = purchase.Split("x");
        var buyAmount = int.Parse(splitPurchase[0]);
        var productPriceInUsd = decimal.Parse(splitPurchase[1]);
        var coins = new Dictionary<int, int>();
        foreach (var coinRaw in centsBank.Split("|"))
        {
            var coinAndAmount = coinRaw.Split("x").Select(int.Parse).ToArray();
            coins.Add(coinAndAmount[1], coinAndAmount[0]);
        }

        $"Given {centsBank} coins available".x(() =>
        {
            _coinBankRepo.GetAvailableCoins(String.Empty).ReturnsForAnyArgs(coins);
        });


        Func<Task<PurchaseResult>>? purchaseFn = null;
        $"When attempting to purchase {buyAmount} products at ${productPriceInUsd} per item (unlimited stock)".x(() =>
        {
            _productRepo.GetProductByName(string.Empty).ReturnsForAnyArgs(new Product()
            {
                Cost = productPriceInUsd,
                AmountAvailable = Int32.MaxValue,
            });
            purchaseFn = () => _vendingMachineService.PurchaseProduct(new PurchaseRequest()
                { DesiredAmount = buyAmount });
        });


        $"The attempt should fail with an explicit exception".x(async () =>
        {
            try
            {
                await purchaseFn!();
            }
            catch (Exception e)
            {
                e.Message.Should().Be(expectedErrorMessage);
            }
        });
    }


    [Scenario]
    public void ProductAmountIsUpdated()
    {
        $"Given 5 products are available".x(() =>
        {
            _productRepo.GetProductByName(string.Empty).ReturnsForAnyArgs(new Product()
                { AmountAvailable = 5, Cost = 10 });
        });


        $"When attempting to purchase 3 with enough funds".x(async () =>
        {
            _coinBankRepo.GetAvailableCoins(String.Empty).ReturnsForAnyArgs(new Dictionary<int, int> { { 100, 100 } });
            await _vendingMachineService.PurchaseProduct(new PurchaseRequest { DesiredAmount = 3 });
        });

        "The product available amount will be set to 2".x(async () =>
        {
            await _productRepo.Received(1).SetProductAmount(Arg.Any<string>(), 2);
        });
    }
}