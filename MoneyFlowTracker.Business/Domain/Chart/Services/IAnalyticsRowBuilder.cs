using MoneyFlowTracker.Business.Domain.Chart.UseCases;

namespace MoneyFlowTracker.Business.Domain.Chart.Services;
public interface IAnalyticsRowBuilder
{
    IEnumerable<IAnalyticsRow> Build(IEnumerable<IAnalyticsChart> analyticsCharts);
}