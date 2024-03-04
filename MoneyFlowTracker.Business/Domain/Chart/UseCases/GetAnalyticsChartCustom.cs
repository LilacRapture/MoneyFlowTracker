namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
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

public class GetAnalyticsChartCustomQueryRequestHandler(IDataContext dataContext) : IRequestHandler<GetAnalyticsChartQueryRequest, IEnumerable<IAnalyticsChart>>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task<IEnumerable<IAnalyticsChart>> Handle(GetAnalyticsChartQueryRequest request, CancellationToken cancellationToken)
    {

        return analyticsCharts;
    }

    private async Task<IEnumerable<IAnalyticsChart>> CalculateCustomIncome(DateOnly date, CancellationToken cancellationToken)
    {
        var netTerminalCategoryId = Guid.Parse("");
        var netDeliveryCategoryId = Guid.Parse("");
        var customIncomeCategories = new Guid[]
        {
            Categories.Income,
            netTerminalCategoryId,
            netDeliveryCategoryId,
        };

        var items = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == date.Year && customIncomeCategories.Contains(item.CategoryId))
            .ToListAsync(cancellationToken: cancellationToken)
        ;
    }
}
