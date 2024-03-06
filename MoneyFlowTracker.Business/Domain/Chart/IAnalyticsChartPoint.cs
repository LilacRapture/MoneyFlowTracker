namespace MoneyFlowTracker.Business.Domain.Chart;


public interface IAnalyticsChartPoint
{
    int AmountCents { get; }
    DateOnly StartDate { get; }
    DateOnly EndDate { get; }
}
