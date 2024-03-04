namespace MoneyFlowTracker.Business.Domain.Chart;

using MoneyFlowTracker.Business.Domain.Category;


public class AnalyticsChart : IAnalyticsChart
{
    public required CategoryModel Category { get; set; }
    public IEnumerable<IAnalyticsPeriod> Weeks { get; set; } = [];
    public IEnumerable<IAnalyticsPeriod> Months { get; set; } = [];
    public IEnumerable<IAnalyticsPeriod> Quarters { get; set; } = [];
}
