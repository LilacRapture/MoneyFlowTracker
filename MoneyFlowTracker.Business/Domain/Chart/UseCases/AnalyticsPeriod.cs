namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;
using System.Collections.Generic;

public class AnalyticsPeriod : IAnalyticsPeriod
{
    public IEnumerable<IAnalyticsChartPoint> ChartPoints { get; set; } = [];
    public int? AmountCents { get; set; } = null;
    public int? ChangePercent { get; set; } = null;

}
