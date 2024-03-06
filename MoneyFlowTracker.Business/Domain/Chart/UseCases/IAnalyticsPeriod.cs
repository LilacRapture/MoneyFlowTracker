namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;

public interface IAnalyticsPeriod
{
    int? AmountCents { get; set; }
    int? ChangePercent { get; set; }
    IEnumerable<IAnalyticsChartPoint> ChartPoints { get; set; }
}