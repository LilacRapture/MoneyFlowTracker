namespace MoneyFlowTracker.Business.Domain.Item;

using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.NetItem;
using System;

public class ItemModel
{
    public Guid Id { get; set; }
    public int AmountCents { get; set; }
    public string? Name { get; set; } = null;
    public DateOnly CreatedDate { get; set; }

    public Guid CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;

    public ItemModel(NetItemModel netItem)
    {
        Id = netItem.Id;
        AmountCents = netItem.AmountCents;
        Name = netItem.Name;
        CreatedDate = netItem.CreatedDate;

        CategoryId = netItem.CategoryId;
        Category = netItem.Category;
    }
}
