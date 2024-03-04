namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Util;
using MoneyFlowTracker.Business.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class GetAnalyticsChartQueryRequest : IRequest<IEnumerable<IAnalyticsChart>>
{
    public DateOnly Date { get; set; }
}

public class GetAnalyticsChartQueryRequestHandler(IDataContext dataContext) : IRequestHandler<GetAnalyticsChartQueryRequest, IEnumerable<IAnalyticsChart>>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task<IEnumerable<IAnalyticsChart>> Handle(GetAnalyticsChartQueryRequest request, CancellationToken cancellationToken)
    {
        var items = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == request.Date.Year)
            .ToListAsync(cancellationToken: cancellationToken)
        ;

        var analyticsCharts = new List<AnalyticsChart>();
        var categories = await _dataContext.Category.ToListAsync(cancellationToken: cancellationToken);
        var analyticsDaysByCategoryId = new Dictionary<Guid, List<AnalyticsPeriod>>();
        
        foreach (var category in categories.Where(c => c.Id != Categories.Income && c.Id != Categories.Expenses))
        {
            var analyticsDays = new List<AnalyticsPeriod>();
            for (
                var currentDay = new DateOnly(request.Date.Year, 1, 1);
                currentDay < request.Date;
                currentDay = currentDay.AddDays(1)
            )
            {
                var totalByDay = items
                    .Where(i => i.CreatedDate == currentDay)
                    .Where(i => i.Category.Id == category.Id || i.Category.ParentCategoryId == category.Id)
                    .Sum(i => i.AmountCents);
                analyticsDays.Add(AnalyticsPeriod.Create(totalByDay, currentDay, currentDay));
            }

            analyticsDaysByCategoryId.Add(category.Id, analyticsDays);
        }

        foreach (var category in categories.Where(c => c.Id == Categories.Income || c.Id == Categories.Expenses))
        {
            var analyticsDays = new List<AnalyticsPeriod>();
            for (
                var currentDay = new DateOnly(request.Date.Year, 1, 1);
                currentDay < request.Date;
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
                analyticsDays.Add(AnalyticsPeriod.Create(categoryTotalByDay + subcategoryTotalByDay, currentDay, currentDay));
            }

            analyticsDaysByCategoryId.Add(category.Id, analyticsDays);
        }

        foreach (var category in categories)
        {
            var analyticsWeeks = new List<AnalyticsPeriod>();
            var analyticsMonths = new List<AnalyticsPeriod>();
            var analyticsQuarters = new List<AnalyticsPeriod>();

            for (
                var currentWeekStartDate = new DateOnly(request.Date.Year, 1, 1);
                currentWeekStartDate < request.Date;
                currentWeekStartDate = currentWeekStartDate.AddDays(7)
            )
            {
                var currentWeekNumber = DateHelper.GetWeekOfYear(currentWeekStartDate);
                var totalByWeek = analyticsDaysByCategoryId[category.Id]
                    .Where(d => DateHelper.GetWeekOfYear(d.StartDate) == currentWeekNumber)
                    .Sum(d => d.AmountCents);
                analyticsWeeks.Add(AnalyticsPeriod.CreateWeek(totalByWeek, currentWeekNumber, request.Date.Year));
            }

            for (
                var currentMonthStartDate = new DateOnly(request.Date.Year, 1, 1);
                currentMonthStartDate < request.Date;
                currentMonthStartDate = currentMonthStartDate.AddMonths(1)
            )
            {
                var totalByMonth = analyticsDaysByCategoryId[category.Id]
                    .Where(d => d.StartDate.Month == currentMonthStartDate.Month)
                    .Sum(d => d.AmountCents);
                analyticsMonths.Add(AnalyticsPeriod.CreateMonth(totalByMonth, currentMonthStartDate.Month, request.Date.Year));
            }

            for (
                var currentQuarterStartDate = new DateOnly(request.Date.Year, 1, 1);
                currentQuarterStartDate < request.Date;
                currentQuarterStartDate = currentQuarterStartDate.AddMonths(3)
            )
            {
                var currentQuarterEndDate = currentQuarterStartDate.AddMonths(3).AddDays(-1);
                var totalByQuarter = analyticsDaysByCategoryId[category.Id]
                    .Where(d => d.StartDate.Month >= currentQuarterStartDate.Month && d.StartDate.Month <= currentQuarterEndDate.Month)
                    .Sum(d => d.AmountCents);
                analyticsQuarters.Add(AnalyticsPeriod.Create(totalByQuarter, currentQuarterStartDate, currentQuarterEndDate));
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
