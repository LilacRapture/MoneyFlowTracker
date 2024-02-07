namespace MoneyFlowTracker.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Item;
using MoneyFlowTracker.Business.Util.Data;
using System.Threading;
using System.Threading.Tasks;

public class MoneyFlowTrackerDbContext : DbContext, IDataContext
{
    public MoneyFlowTrackerDbContext(DbContextOptions<MoneyFlowTrackerDbContext> options) : base(options)
    {
    }

    public DbSet<ItemModel> Items { get; set; }
    public DbSet<CategoryModel> Category { get; set; }

    async Task IDataContext.SaveChanges(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // <-- ItemModel -->
        var itemBuilder = modelBuilder.Entity<ItemModel>();

        // Key
        itemBuilder.HasKey(i => i.Id);
        itemBuilder
            .HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId)
            .IsRequired()
        ;
        
        // </- ItemModel -->

        // <-- CategoryModel -->
        var categoryBuilder = modelBuilder.Entity<CategoryModel>();

        // Key
        categoryBuilder.HasKey(p => p.Id);
        // </- CategoryModel -->
    }

}
