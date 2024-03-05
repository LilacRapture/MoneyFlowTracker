namespace MoneyFlowTracker.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Balance;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Item;
using MoneyFlowTracker.Business.Domain.NetItem;
using MoneyFlowTracker.Business.Util.Data;
using System.Threading;
using System.Threading.Tasks;


public class MoneyFlowTrackerDbContext(DbContextOptions<MoneyFlowTrackerDbContext> options) : DbContext(options), IDataContext
{
    public DbSet<ItemModel> Items { get; set; }
    public DbSet<NetItemModel> NetItems { get; set; }
    public DbSet<BalanceModel> Balances { get; set; }
    public DbSet<CategoryModel> Category { get; set; }

    async Task IDataContext.SaveChanges(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // <-- Items -->
        var itemBuilder = modelBuilder.Entity<ItemModel>().ToTable("Items");

        // Key
        itemBuilder.HasKey(i => i.Id);

        itemBuilder
            .HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId)
            .IsRequired()
        ;
        // </- Items -->

        // <-- NetItems -->
        var netItemBuilder = modelBuilder.Entity<NetItemModel>().ToTable("NetItems");

        // Key
        netItemBuilder.HasKey(i => i.Id);

        netItemBuilder
            .HasOne(i => i.Category)
            .WithMany(c => c.NetItems)
            .HasForeignKey(i => i.CategoryId)
            .IsRequired()
        ;
        // </- NetItems -->

        // <-- CategoryModel -->
        var categoryBuilder = modelBuilder.Entity<CategoryModel>();

        // Key
        categoryBuilder.HasKey(c => c.Id);

        categoryBuilder
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.ChildCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .IsRequired(false)
        ;
        // </- CategoryModel -->

        // <-- Balances -->
        var balanceBuilder = modelBuilder.Entity<BalanceModel>().ToTable("Balances");

        // Key
        balanceBuilder.HasKey(i => i.Id);
        // </- Balances -->
    }

}
