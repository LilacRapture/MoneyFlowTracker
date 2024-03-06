namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;
using MoneyFlowTracker.Business.Domain.Category;

public class AnalyticsRow : IAnalyticsRow
{
    public required CategoryModel Category { get; set; }
    public required IAnalyticsPeriod Weekly { get; set; }
    public required IAnalyticsPeriod Monthly { get; set; }
    public required IAnalyticsPeriod Quarterly { get; set; }
}
