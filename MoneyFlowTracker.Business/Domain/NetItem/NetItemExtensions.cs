namespace MoneyFlowTracker.Business.Domain.NetItem;

using MoneyFlowTracker.Business.Domain.Item;


public static class NetItemExtensions
{
    public static ItemModel AsItemModel(NetItemModel netItem) => new()
    {
        Id = netItem.Id,
        AmountCents = netItem.AmountCents,
        Name = netItem.Name,
        CreatedDate = netItem.CreatedDate,
        CategoryId = netItem.CategoryId,
        Category = netItem.Category,
    };
}