using System.ComponentModel.DataAnnotations;

namespace VendingMachine.Domain.CoinBank;

public class PurchaseRequest
{
    [Required]
    public string ProductName { get; set; } = null!;
    
    [Range(1,100)]
    public int DesiredAmount { get; set; }
}