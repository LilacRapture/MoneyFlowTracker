namespace MoneyFlowTracker.Business.Domain.Item;

using MoneyFlowTracker.Business.Domain.Category;
using System;

public class ItemModel
{
    public Guid Id { get; set; }
    public int AmountCents { get; set; }
    public string? Name { get; set; } = null;
    public DateOnly CreatedDate { get; set; }

    public Guid CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;
}
