namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Util;
using MoneyFlowTracker.Business.Util.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class GetAnalyticsChartQueryRequest : IRequest<IEnumerable<IAnalyticsChart>>
{
    public DateOnly Date { get; set; }
}

public interface IAnalyticsPeriod
{
    int AmountCents { get; }
    DateOnly StartDate { get; }
    DateOnly EndDate { get; }
}

public class AnalyticsPeriod : IAnalyticsPeriod
{
    public int AmountCents { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public static AnalyticsPeriod CreateWeek(int amountCents, int weekNumber, int year)
    {
        var startDate = DateHelper.GetWeekDateOnly(year, weekNumber);

        return new AnalyticsPeriod
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = startDate.AddDays(6),
        }; 
    }
    public static AnalyticsPeriod CreateMonth(int amountCents, int month, int year)
    {
        var startDate = new DateOnly(year, month, 1);

        return new AnalyticsPeriod
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = startDate.AddMonths(1).AddDays(-1),
        };
    }

    public static AnalyticsPeriod Create(int amountCents, DateOnly startDate, DateOnly endDate)
    {
        return new AnalyticsPeriod
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = endDate,
        };
    }
}

public interface IAnalyticsChart
{
    CategoryModel Category { get; }
    IEnumerable<IAnalyticsPeriod> Weeks {  get; }
    IEnumerable<IAnalyticsPeriod> Months { get; }
    IEnumerable<IAnalyticsPeriod> Quarters { get; }
}

public class AnalyticsChart : IAnalyticsChart
{
    public required CategoryModel Category { get; set; }
    public IEnumerable<IAnalyticsPeriod> Weeks { get; set; } = Enumerable.Empty<IAnalyticsPeriod>();
    public IEnumerable<IAnalyticsPeriod> Months { get; set; } = Enumerable.Empty<IAnalyticsPeriod>();
    public IEnumerable<IAnalyticsPeriod> Quarters { get; set; } = Enumerable.Empty<IAnalyticsPeriod>();
}


public class GetAnalyticsChartQueryRequestHandler : IRequestHandler<GetAnalyticsChartQueryRequest, IEnumerable<IAnalyticsChart>>
{
    private readonly IDataContext _dataContext;
    public GetAnalyticsChartQueryRequestHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<IEnumerable<IAnalyticsChart>> Handle(GetAnalyticsChartQueryRequest request, CancellationToken cancellationToken)
    {
        var items = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == request.Date.Year)
            .ToListAsync()
        ;

        var analyticsCharts = new List<AnalyticsChart>();
        var categories = await _dataContext.Category.ToListAsync();
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
