
namespace MoneyFlowTracker.Business.Domain.Chart.Services;

public interface ICustomRevenueService
{
    Task<IEnumerable<IAnalyticsChart>> CreateCustomIncomeCharts(DateOnly date);
}