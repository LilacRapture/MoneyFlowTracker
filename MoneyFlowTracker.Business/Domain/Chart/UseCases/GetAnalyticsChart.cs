namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Chart.Services;
using MoneyFlowTracker.Business.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GetAnalyticsChartQueryRequest : IRequest<IEnumerable<IAnalyticsRow>>
{
    public DateOnly Date { get; set; }
}

public class GetAnalyticsChartQueryRequestHandler(
    IDataContext dataContext,
    IAnalyticsChartBuilder analyticsChartBuilder
)
  : IRequestHandler<GetAnalyticsChartQueryRequest, IEnumerable<IAnalyticsRow>>
{
    private readonly IDataContext _dataContext = dataContext;
    private readonly IAnalyticsChartBuilder _analyticsChartBuilder = analyticsChartBuilder;

    public async Task<IEnumerable<IAnalyticsRow>> Handle(GetAnalyticsChartQueryRequest request, CancellationToken cancellationToken)
    {
        var items = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == request.Date.Year)
            .ToListAsync(cancellationToken: cancellationToken)
        ;

        var categories = await _dataContext.Category.ToListAsync(cancellationToken: cancellationToken);
        var analyticsCharts = _analyticsChartBuilder.Build(items, categories, request.Date);

        var analyticsRows = analyticsCharts.Select(c => new AnalyticsRow
        {
            Category = c.Category,
            Weekly = MapChartPointsToAnalyticsPeriod(c.Weeks),
            Monthly = MapChartPointsToAnalyticsPeriod(c.Months),
            Quarterly = MapChartPointsToAnalyticsPeriod(c.Quarters),
        });

        return analyticsRows;
    }
    private static AnalyticsPeriod MapChartPointsToAnalyticsPeriod(IEnumerable<IAnalyticsChartPoint> analyticsChartPoints)
    {
        var sortedAmountCents = analyticsChartPoints.OrderByDescending(p => p.StartDate).Select(p => p.AmountCents);
        var lastAmountCents = sortedAmountCents.FirstOrDefault();
        var previousLastAmountCents = sortedAmountCents.ElementAtOrDefault(1);
        var changePercent = 0;
        if (lastAmountCents != 0)
        {
            changePercent = (previousLastAmountCents - lastAmountCents) / lastAmountCents * 100;
        }

        return new AnalyticsPeriod
        {
            ChartPoints = analyticsChartPoints,
            AmountCents = lastAmountCents,
            ChangePercent = changePercent,
        };
    }
}
