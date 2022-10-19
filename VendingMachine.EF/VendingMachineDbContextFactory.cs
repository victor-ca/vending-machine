using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VendingMachine.EF;

public class VendingMachineDbContextFactory: IDesignTimeDbContextFactory<VendingMachineDbContext>
{
    public VendingMachineDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<VendingMachineDbContext>()
            .UseSqlite("DataSource=../db.sqlite");
        
        var context = new VendingMachineDbContext(optionsBuilder.Options);
        // context.Database.Migrate();
        return context;
    }
}