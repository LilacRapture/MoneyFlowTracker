
namespace MoneyFlowTracker.Business.Domain.Chart.Services;

public interface ICustomRevenueService
{
    IEnumerable<IAnalyticsChart> CreateCustomIncomeCharts(DateOnly date);
}