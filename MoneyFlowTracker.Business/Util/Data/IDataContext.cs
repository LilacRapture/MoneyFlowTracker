namespace MoneyFlowTracker.Business.Util.Data;

using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Item;
using MoneyFlowTracker.Business.Domain.NetItem;

public interface IDataContext
{
    DbSet<ItemModel> Items { get; }
    DbSet<NetItemModel> NetItems { get; }
    DbSet<CategoryModel> Category { get; }

    Task SaveChanges(CancellationToken cancellationToken);
}
