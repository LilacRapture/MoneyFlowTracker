namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Chart.Services;
using MoneyFlowTracker.Business.Domain.Item;
using MoneyFlowTracker.Business.Util;
using MoneyFlowTracker.Business.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class GetAnalyticsChartCustomQueryRequest : IRequest<IEnumerable<IAnalyticsChart>>
{
    public DateOnly Date { get; set; }
}

public class GetAnalyticsChartCustomQueryRequestHandler(
    IDataContext dataContext,
    IAnalyticsChartBuilder analyticsChartBuilder
)
  : IRequestHandler<GetAnalyticsChartQueryRequest, IEnumerable<IAnalyticsChart>>
{
    private readonly IDataContext _dataContext = dataContext;
    private readonly IAnalyticsChartBuilder _analyticsChartBuilder = analyticsChartBuilder;

    public async Task<IEnumerable<IAnalyticsChart>> Handle(GetAnalyticsChartQueryRequest request, CancellationToken cancellationToken)
    {
        return await CalculateCustomIncome(request.Date, cancellationToken);
    }

    private async Task<IEnumerable<IAnalyticsChart>> CalculateCustomIncome(DateOnly date, CancellationToken cancellationToken)
    {
        var grossCashCategoryId = Guid.Parse("");
        var netTerminalCategoryId = Guid.Parse("");
        var netDeliveryCategoryId = Guid.Parse("");

        var customGrossIncomeCategoryIds = new Guid[]
        {
            Categories.Income,
            grossCashCategoryId,
        };
        var grossItems = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == date.Year && customGrossIncomeCategoryIds.Contains(item.CategoryId))
            .ToListAsync(cancellationToken: cancellationToken)
        ;

        var customNetIncomeCategoryIds = new Guid[]
        {
            netTerminalCategoryId,
            netDeliveryCategoryId,
        };
        var netItems = await _dataContext.NetItems
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == date.Year && customNetIncomeCategoryIds.Contains(item.CategoryId))
            .ToListAsync(cancellationToken: cancellationToken)
        ;

        var allItems = grossItems.Concat(netItems.Select(ni => new ItemModel(ni)));


        var allCategoryIds = customGrossIncomeCategoryIds.Concat(customNetIncomeCategoryIds);
        var categories = await _dataContext.Category
            .Where(c => allCategoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken: cancellationToken)
        ;


        return _analyticsChartBuilder.Build(date, allItems, categories);
    }
}
