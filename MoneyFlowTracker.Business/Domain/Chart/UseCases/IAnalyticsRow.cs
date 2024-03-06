namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;
using MoneyFlowTracker.Business.Domain.Category;

public interface IAnalyticsRow
{
    CategoryModel Category { get; set; }
    IAnalyticsPeriod Weekly {  get; set; }
    IAnalyticsPeriod Monthly { get; set; }
    IAnalyticsPeriod Quarterly { get; set; }

}
