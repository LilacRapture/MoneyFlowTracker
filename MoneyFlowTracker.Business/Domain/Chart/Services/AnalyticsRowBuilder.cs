namespace MoneyFlowTracker.Business.Domain.Chart.Services;

using MoneyFlowTracker.Business.Domain.Chart.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AnalyticsRowBuilder : IAnalyticsRowBuilder
{
    public IEnumerable<IAnalyticsRow> Build(IEnumerable<IAnalyticsChart> analyticsCharts)
    {
        var analyticsRows = analyticsCharts.Select(c => new AnalyticsRow
        {
            Category = c.Category,
            Weekly = MapChartPointsToAnalyticsPeriod(c.Weeks),
            Monthly = MapChartPointsToAnalyticsPeriod(c.Months),
            Quarterly = MapChartPointsToAnalyticsPeriod(c.Quarters),
        });

        return analyticsRows;
    }
    private static IAnalyticsPeriod MapChartPointsToAnalyticsPeriod(IEnumerable<IAnalyticsChartPoint> analyticsChartPoints)
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
