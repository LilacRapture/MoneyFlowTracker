namespace MoneyFlowTracker.Business.Domain.Chart.Services;

using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Item;
using MoneyFlowTracker.Business.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using static NetItem.NetItemExtensions;

public class CustomRevenueService(
    IDataContext dataContext,
    IAnalyticsChartBuilder analyticsChartBuilder
) 
    : ICustomRevenueService
{
    public async Task<IEnumerable<IAnalyticsChart>> CreateCustomIncomeCharts(DateOnly date)
    {
        // Prepare Category Ids
        var grossItemCategoryIds = new Guid[]
        {
            Categories.Revenue,
            Categories.Cash,
        };
        var netItemCategoryIds = new Guid[]
        {
            Categories.Terminal,
            Categories.Delivery,
        };


        // Prepare Categories
        var allCategoryIds = grossItemCategoryIds.Concat(netItemCategoryIds);
        var categories = await dataContext.Category
            .Where(c => allCategoryIds.Contains(c.Id))
            .ToListAsync()
        ;
        var customNamedCategories = categories.Select(MapCategoryToCustomCategory);


        // Prepare Items
        var grossItems = await dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == date.Year && grossItemCategoryIds.Contains(item.CategoryId))
            .ToListAsync()
        ;
        var netItems = await dataContext.NetItems
            .Include(i => i.Category)
            .Where(item =>
                item.CreatedDate.Year == date.Year &&
                (
                    netItemCategoryIds.Contains(item.Category.Id) ||
                    netItemCategoryIds.Contains(item.Category.ParentCategoryId.GetValueOrDefault(Guid.Empty))
                )
            )
            .ToListAsync()
        ;
        var allItems = grossItems
            .Concat(netItems.Select(AsItemModel))
            .Select(i => MapItemToCustomItem(
                i,
                customNamedCategories.Single(c => c.Id == BuildCustomCategoryId(i.CategoryId))
            ))
        ;


        return analyticsChartBuilder.Build(allItems, customNamedCategories, date);
    }

    private static Guid BuildCustomCategoryId(Guid categoryId) =>
        categoryId.ToString() switch
        {
            Categories.Revenue_String => Categories.CustomRevenue,
            Categories.Cash_String => Categories.CustomRevenue_GrossCash,
            Categories.Terminal_String => Categories.CustomRevenue_NetTerminal,
            Categories.Delivery_String => Categories.CustomRevenue_NetDelivery,
            Categories.Delivery_Wolt_String => Categories.CustomRevenue_NetDelivery,
            Categories.Delivery_Bolt_String => Categories.CustomRevenue_NetDelivery,
            Categories.Delivery_Glovo_String => Categories.CustomRevenue_NetDelivery,
            _ => throw new Exception($"No custom id for category '{categoryId}'"),
        }
    ;

    private static Guid? BuildCustomCategoryParentId(Guid? categoryId) =>
        categoryId == null ? null : Categories.CustomRevenue
    ;

    private static string BuildCustomCategoryName(Guid categoryId) =>
        categoryId.ToString() switch
        {
            Categories.CustomRevenue_String => "Чистый Приход",
            Categories.CustomRevenue_GrossCash_String => "Грязный Нал",
            Categories.CustomRevenue_NetTerminal_String => "Чистый Терминал",
            Categories.CustomRevenue_NetDelivery_String => "Чистая Доставка",
            _ => throw new Exception($"No custom name for category '{categoryId}'"),
        }
    ;

    private static ItemModel MapItemToCustomItem(ItemModel item, CategoryModel customCategory) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            AmountCents = item.AmountCents,
            CreatedDate = item.CreatedDate,
            CategoryId = customCategory.Id,
            Category = customCategory,
        }
    ;

    private static CategoryModel MapCategoryToCustomCategory(CategoryModel category)
    {
        var customCategoryId = BuildCustomCategoryId(category.Id);

        return new()
        {
            Id = customCategoryId,
            Name = BuildCustomCategoryName(customCategoryId),
            IsIncome = category.IsIncome,
            ParentCategoryId = BuildCustomCategoryParentId(category.ParentCategoryId),
        };
    }
}
