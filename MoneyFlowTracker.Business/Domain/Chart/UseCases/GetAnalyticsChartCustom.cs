namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Chart.Services;
using MoneyFlowTracker.Business.Domain.Item;
using MoneyFlowTracker.Business.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NetItem.NetItemExtensions;

public class GetAnalyticsChartCustomQueryRequest : IRequest<IEnumerable<IAnalyticsRow>>
{
    public DateOnly Date { get; set; }
}

public class GetAnalyticsChartCustomQueryRequestHandler(
    IDataContext dataContext,
    IAnalyticsChartBuilder analyticsChartBuilder,
    IAnalyticsRowBuilder analyticsRowBuilder
)
  : IRequestHandler<GetAnalyticsChartCustomQueryRequest, IEnumerable<IAnalyticsRow>>
{
    private readonly IDataContext _dataContext = dataContext;
    private readonly IAnalyticsChartBuilder _analyticsChartBuilder = analyticsChartBuilder;
    private readonly IAnalyticsRowBuilder _analyticsRowBuilder = analyticsRowBuilder;

    public async Task<IEnumerable<IAnalyticsRow>> Handle(GetAnalyticsChartCustomQueryRequest request, CancellationToken cancellationToken)
    {
        var customAnalyticsCharts = await BuildCustomIncomeCharts(request.Date, cancellationToken);
        var analyticsRows = _analyticsRowBuilder.Build(customAnalyticsCharts);

        return analyticsRows;
    }

    private async Task<IEnumerable<IAnalyticsChart>> BuildCustomIncomeCharts(DateOnly date, CancellationToken cancellationToken)
    {
        // Prepare Category Ids
        var grossItemCategoryIds = new Guid[]
        {
            Categories.Income,
            Categories.Cash,
        };
        var netItemCategoryIds = new Guid[]
        {
            Categories.Terminal,
            Categories.Delivery,
        };


        // Prepare Categories
        var allCategoryIds = grossItemCategoryIds.Concat(netItemCategoryIds);
        var categories = await _dataContext.Category
            .Where(c => allCategoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken: cancellationToken)
        ;
        var customNamedCategories = categories.Select(c => new CategoryModel
        {
            Id = BuildCustomCategoryId(c.Id),
            Name = BuildCustomCategoryName(c.Id.ToString()),
            IsIncome = c.IsIncome,
            ParentCategoryId = BuildCustomCategoryParentId(c.ParentCategoryId),
        });


        // Prepare Items
        var grossItems = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == date.Year && grossItemCategoryIds.Contains(item.CategoryId))
            .ToListAsync(cancellationToken: cancellationToken)
        ;
        var netItems = await _dataContext.NetItems
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
            .ToListAsync(cancellationToken: cancellationToken)
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

    private const string CustomIncomeIdString = "b1488d86-353a-4e89-a890-de1ecb6ba9bb";
    private static Guid BuildCustomCategoryId(Guid categoryId) =>
        categoryId.ToString() switch
        {
            Categories.IncomeString => Guid.Parse(CustomIncomeIdString),
            Categories.CashString => Guid.Parse("8bbce42f-f440-4784-8fe1-3c70da523a4e"),
            Categories.TerminalString => Guid.Parse("a7748246-cd7d-4dcb-b217-5a5c100cf0a9"),
            Categories.DeliveryString => Guid.Parse("e56f662d-696b-47ca-952e-fa2b9271584d"),
            _ => throw new Exception($"No custom id for category '{categoryId}'"),
        }
    ;

    private static Guid? BuildCustomCategoryParentId(Guid? categoryId) =>
        categoryId == null ? null : Guid.Parse(CustomIncomeIdString)
    ;

    private static string BuildCustomCategoryName(string categoryId) =>
        categoryId switch
        {
            Categories.IncomeString => "Чистый Приход",
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



