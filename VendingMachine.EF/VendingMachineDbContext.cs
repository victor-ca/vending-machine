using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VendingMachine.EF.Products;
using VendingMachine.EF.Users;


namespace VendingMachine.EF;
public class VendingMachineDbContext :  IdentityDbContext<VendingMachineUserDpo>
{
    public VendingMachineDbContext(DbContextOptions<VendingMachineDbContext> options) : base(options) { }

    public DbSet<ProductDpo> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProductDpo>()
            .HasKey(t => new { t.Name, t.SellerId});


        modelBuilder.Entity<ProductDpo>()
            .HasOne<VendingMachineUserDpo>()
            .WithMany()
            .HasForeignKey(p=>p.SellerId)
            .HasPrincipalKey(s => s.UserName);

    }
}