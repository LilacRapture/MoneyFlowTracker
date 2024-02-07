namespace MoneyFlowTracker.Business.Util.Data;

using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Item;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public interface IDataContext
{
    DbSet<ItemModel> Items { get; }
    DbSet<CategoryModel> Category { get; }

    Task SaveChanges(CancellationToken cancellationToken);
}
