namespace MoneyFlowTracker.Business.Domain.Chart.Services;

using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Item;


public interface IAnalyticsChartBuilder
{
    IEnumerable<AnalyticsChart> Build(DateOnly date, IEnumerable<ItemModel> items, IEnumerable<CategoryModel> categories);
}