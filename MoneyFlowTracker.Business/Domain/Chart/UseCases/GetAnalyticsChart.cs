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


public class GetAnalyticsChartQueryRequest : IRequest<IEnumerable<IAnalyticsChart>>
{
    public DateOnly Date { get; set; }
}

public class GetAnalyticsChartQueryRequestHandler(
    IDataContext dataContext,
    IAnalyticsChartBuilder analyticsChartBuilder
)
  : IRequestHandler<GetAnalyticsChartQueryRequest, IEnumerable<IAnalyticsChart>>
{
    private readonly IDataContext _dataContext = dataContext;
    private readonly IAnalyticsChartBuilder _analyticsChartBuilder = analyticsChartBuilder;

    public async Task<IEnumerable<IAnalyticsChart>> Handle(GetAnalyticsChartQueryRequest request, CancellationToken cancellationToken)
    {
        var items = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == request.Date.Year)
            .ToListAsync(cancellationToken: cancellationToken)
        ;

        var categories = await _dataContext.Category.ToListAsync(cancellationToken: cancellationToken);
        var analyticsCharts = _analyticsChartBuilder.Build(request.Date, items, categories);

        return analyticsCharts;
    }
}
