namespace MoneyFlowTracker.Business.Domain.Chart;

using MoneyFlowTracker.Business.Domain.Category;


public interface IAnalyticsChart
{
    CategoryModel Category { get; }
    IEnumerable<IAnalyticsPeriod> Weeks {  get; }
    IEnumerable<IAnalyticsPeriod> Months { get; }
    IEnumerable<IAnalyticsPeriod> Quarters { get; }
}
