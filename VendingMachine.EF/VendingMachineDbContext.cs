using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VendingMachine.EF.Coins;
using VendingMachine.EF.Products;
using VendingMachine.EF.Users;


namespace VendingMachine.EF;
public class VendingMachineDbContext :  IdentityDbContext<VendingMachineUserDpo>
{
    public VendingMachineDbContext(DbContextOptions<VendingMachineDbContext> options) : base(options) { }

    public DbSet<ProductDpo> Products { get; set; } = null!;
    public DbSet<UserCoins> CoinBank { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProductDpo>()
            .HasKey(t => t.Name);
            
        modelBuilder.Entity<UserCoins>()
            .HasKey(t => new { t.CentDenominator, t.UserName});


        modelBuilder.Entity<ProductDpo>()
            .HasOne<VendingMachineUserDpo>()
            .WithMany()
            .HasForeignKey(p=>p.SellerId)
            .HasPrincipalKey(s => s.UserName);
        
        modelBuilder.Entity<ProductDpo>()
            .HasOne<VendingMachineUserDpo>()
            .WithMany()
            .HasForeignKey(p=>p.SellerId)
            .HasPrincipalKey(s => s.UserName);
            
        modelBuilder.Entity<UserCoins>()
            .HasOne<VendingMachineUserDpo>()
            .WithMany()
            .HasForeignKey(p=>p.UserName)
            .HasPrincipalKey(s => s.UserName);

    }
}