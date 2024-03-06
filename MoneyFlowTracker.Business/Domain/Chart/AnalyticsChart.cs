namespace MoneyFlowTracker.Business.Domain.Chart;

using MoneyFlowTracker.Business.Domain.Category;


public class AnalyticsChart : IAnalyticsChart
{
    public required CategoryModel Category { get; set; }
    public IEnumerable<IAnalyticsChartPoint> Weeks { get; set; } = [];
    public IEnumerable<IAnalyticsChartPoint> Months { get; set; } = [];
    public IEnumerable<IAnalyticsChartPoint> Quarters { get; set; } = [];
}
