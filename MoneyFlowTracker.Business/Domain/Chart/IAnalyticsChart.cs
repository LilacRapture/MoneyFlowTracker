namespace MoneyFlowTracker.Business.Domain.Chart;

using MoneyFlowTracker.Business.Domain.Category;


public interface IAnalyticsChart
{
    CategoryModel Category { get; }
    IEnumerable<IAnalyticsChartPoint> Weeks {  get; }
    IEnumerable<IAnalyticsChartPoint> Months { get; }
    IEnumerable<IAnalyticsChartPoint> Quarters { get; }
}
