using Microsoft.EntityFrameworkCore;
using VendingMachine.EF.Products;
using VendingMachine.EF.Users;


namespace VendingMachine.EF;
public class VendingMachineDbContext : DbContext
{
    public VendingMachineDbContext(DbContextOptions<VendingMachineDbContext> options) : base(options) { }

    public DbSet<VendingMachineUserDpo> Users { get; set; }
    public DbSet<ProductDpo> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDpo>()
            .HasKey(t => new { t.Name, t.SellerId});

        modelBuilder.Entity<VendingMachineUserDpo>()
            .HasKey(t => t.UserName);
        
        modelBuilder.Entity<ProductDpo>()
            .HasOne<VendingMachineUserDpo>()
            .WithMany()
            .HasForeignKey(s => s.SellerId);
            
    }
}