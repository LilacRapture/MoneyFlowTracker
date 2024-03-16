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
    private readonly IDataContext _dataContext = dataContext;
    private readonly IAnalyticsChartBuilder _analyticsChartBuilder = analyticsChartBuilder;
    public IEnumerable<IAnalyticsChart> CreateCustomIncomeCharts(DateOnly date)
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
        var categories = _dataContext.Category
            .Where(c => allCategoryIds.Contains(c.Id))
            .ToList()
        ;
        var customNamedCategories = categories.Select(c => new CategoryModel
        {
            Id = BuildCustomCategoryId(c.Id),
            Name = BuildCustomCategoryName(c.Id.ToString()),
            IsIncome = c.IsIncome,
            ParentCategoryId = BuildCustomCategoryParentId(c.ParentCategoryId),
        });


        // Prepare Items
        var grossItems = _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == date.Year && grossItemCategoryIds.Contains(item.CategoryId))
            .ToList()
        ;
        var netItems = _dataContext.NetItems
            .Include(i => i.Category)
            .Where(item =>
                item.CreatedDate.Year == date.Year &&
                (
                    netItemCategoryIds.Contains(item.Category.Id) ||
                    (
                        item.Category.ParentCategoryId != null &&
                        netItemCategoryIds.Contains(item.Category.ParentCategoryId.Value)
            )
            )
            )
            .ToList()
        ;
        var allItems = grossItems
            .Concat(netItems.Select(AsItemModel))
            .Select(i => MapItemToCustomItem(
                i,
                customNamedCategories.Single(c => c.Id == BuildCustomCategoryId(i.CategoryId))
            ))
        ;


        return _analyticsChartBuilder.Build(allItems, customNamedCategories, date);
    }

    private static Guid BuildCustomCategoryId(Guid categoryId) =>
        categoryId.ToString() switch
        {
            Categories.RevenueString => Categories.CustomRevenue,
            Categories.CashString => Guid.Parse("8bbce42f-f440-4784-8fe1-3c70da523a4e"),
            Categories.TerminalString => Guid.Parse("a7748246-cd7d-4dcb-b217-5a5c100cf0a9"),
            Categories.DeliveryString => Guid.Parse("e56f662d-696b-47ca-952e-fa2b9271584d"),
            _ => throw new Exception($"No custom id for category '{categoryId}'"),
        }
    ;

    private static Guid? BuildCustomCategoryParentId(Guid? categoryId) =>
        categoryId == null ? null : Categories.CustomRevenue
    ;

    private static string BuildCustomCategoryName(string categoryId) =>
        categoryId switch
        {
            Categories.RevenueString => "Чистый Приход",
            Categories.CashString => "Грязный Нал",
            Categories.TerminalString => "Чистый Терминал",
            Categories.DeliveryString => "Чистая Доставка",
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
}
