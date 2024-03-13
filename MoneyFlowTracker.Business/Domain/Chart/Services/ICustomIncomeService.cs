
namespace MoneyFlowTracker.Business.Domain.Chart.Services;

public interface ICustomIncomeService
{
    IEnumerable<IAnalyticsChart> CreateCustomIncomeCharts(DateOnly date);
}