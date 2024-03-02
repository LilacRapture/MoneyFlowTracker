namespace MoneyFlowTracker.Business.Domain.NetItem;

using MoneyFlowTracker.Business.Domain.Category;
using System;

public class NetItemModel
{
    public Guid Id { get; set; }
    public int AmountCents { get; set; }
    public string? Name { get; set; } = null;
    public DateOnly CreatedDate { get; set; }

    public Guid CategoryId { get; set; }
    public CategoryModel Category { get; set; } = null!;
}
