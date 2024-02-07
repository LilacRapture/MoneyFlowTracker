namespace MoneyFlowTracker.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class MoneyFlowTrackerDbContextFactory : IDesignTimeDbContextFactory<MoneyFlowTrackerDbContext>
{
    public MoneyFlowTrackerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MoneyFlowTrackerDbContext>();

        optionsBuilder.UseNpgsql("User ID=admin;Password=rgIo9rvRgI2n8v6mpwrqOnWBAB4liatt;Host=dpg-cmt02c21hbls73cm3h50-a.frankfurt-postgres.render.com;Port=5432;Database=maindb_6cag;Pooling=true;");
        return new MoneyFlowTrackerDbContext(optionsBuilder.Options);
    }
}
