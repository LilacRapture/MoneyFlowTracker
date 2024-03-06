namespace MoneyFlowTracker.Business.Domain.Chart.Services;

using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Item;
using MoneyFlowTracker.Business.Util;

public class AnalyticsChartBuilder : IAnalyticsChartBuilder
{
    public IEnumerable<AnalyticsChart> Build(IEnumerable<ItemModel> items, IEnumerable<CategoryModel> categories, DateOnly date)
    {
        var analyticsCharts = new List<AnalyticsChart>();
        var analyticsDaysByCategoryId = new Dictionary<Guid, List<AnalyticsChartPoint>>();

        foreach (var category in categories.Where(c => c.Id != Categories.Income && c.Id != Categories.Expenses))
        {
            var analyticsDays = new List<AnalyticsChartPoint>();
            for (
                var currentDay = new DateOnly(date.Year, 1, 1);
                currentDay < date;
                currentDay = currentDay.AddDays(1)
            )
            {
                var totalByDay = items
                    .Where(i => i.CreatedDate == currentDay)
                    .Where(i => i.Category.Id == category.Id || i.Category.ParentCategoryId == category.Id)
                    .Sum(i => i.AmountCents);
                analyticsDays.Add(AnalyticsChartPoint.Create(totalByDay, currentDay, currentDay));
            }

            analyticsDaysByCategoryId.Add(category.Id, analyticsDays);
        }

        foreach (var category in categories.Where(c => c.Id == Categories.Income || c.Id == Categories.Expenses))
        {
            var analyticsDays = new List<AnalyticsChartPoint>();
            for (
                var currentDay = new DateOnly(date.Year, 1, 1);
                currentDay < date;
                currentDay = currentDay.AddDays(1)
            )
            {
                var categoryTotalByDay = items
                    .Where(i => i.CreatedDate == currentDay)
                    .Where(i => i.Category.Id == category.Id)
                    .Sum(i => i.AmountCents);
                var subcategoryTotalByDay = categories
                    .Where(c => c.ParentCategoryId == category.Id)
                    .Sum(c => analyticsDaysByCategoryId[c.Id]
                        .Where(p => p.StartDate == currentDay)
                        .Sum(p => p.AmountCents)
                    );
                analyticsDays.Add(AnalyticsChartPoint.Create(categoryTotalByDay + subcategoryTotalByDay, currentDay, currentDay));
            }

            analyticsDaysByCategoryId.Add(category.Id, analyticsDays);
        }

        foreach (var category in categories)
        {
            var analyticsWeeks = new List<AnalyticsChartPoint>();
            var analyticsMonths = new List<AnalyticsChartPoint>();
            var analyticsQuarters = new List<AnalyticsChartPoint>();

            for (
                var currentWeekStartDate = new DateOnly(date.Year, 1, 1);
                currentWeekStartDate < date;
                currentWeekStartDate = currentWeekStartDate.AddDays(7)
            )
            {
                var currentWeekNumber = DateHelper.GetWeekOfYear(currentWeekStartDate);
                var totalByWeek = analyticsDaysByCategoryId[category.Id]
                    .Where(d => DateHelper.GetWeekOfYear(d.StartDate) == currentWeekNumber)
                    .Sum(d => d.AmountCents);
                analyticsWeeks.Add(AnalyticsChartPoint.CreateWeek(totalByWeek, currentWeekNumber, date.Year));
            }

            for (
                var currentMonthStartDate = new DateOnly(date.Year, 1, 1);
                currentMonthStartDate < date;
                currentMonthStartDate = currentMonthStartDate.AddMonths(1)
            )
            {
                var totalByMonth = analyticsDaysByCategoryId[category.Id]
                    .Where(d => d.StartDate.Month == currentMonthStartDate.Month)
                    .Sum(d => d.AmountCents);
                analyticsMonths.Add(AnalyticsChartPoint.CreateMonth(totalByMonth, currentMonthStartDate.Month, date.Year));
            }

            for (
                var currentQuarterStartDate = new DateOnly(date.Year, 1, 1);
                currentQuarterStartDate < date;
                currentQuarterStartDate = currentQuarterStartDate.AddMonths(3)
            )
            {
                var currentQuarterEndDate = currentQuarterStartDate.AddMonths(3).AddDays(-1);
                var totalByQuarter = analyticsDaysByCategoryId[category.Id]
                    .Where(d => d.StartDate.Month >= currentQuarterStartDate.Month && d.StartDate.Month <= currentQuarterEndDate.Month)
                    .Sum(d => d.AmountCents);
                analyticsQuarters.Add(AnalyticsChartPoint.Create(totalByQuarter, currentQuarterStartDate, currentQuarterEndDate));
            }

            var analyticsChart = new AnalyticsChart
            {
                Category = category,
                Weeks = analyticsWeeks,
                Months = analyticsMonths,
                Quarters = analyticsQuarters,
            };
            analyticsCharts.Add(analyticsChart);
        }

        return analyticsCharts;
    }
}