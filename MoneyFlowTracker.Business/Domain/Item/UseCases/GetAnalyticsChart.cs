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

    public static AnalyticsPeriod CreateQuarter(int amountCents, int month, int year)
    {
        var startDate = new DateOnly(year, (month - 1) / 3 * 3 + 1, 1);

        return new AnalyticsPeriod
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = startDate.AddMonths(3).AddDays(-1),
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
                var totalByWeek = items
                    .Where(i => DateHelper.GetWeekOfYear(i.CreatedDate) == currentWeekNumber)
                    .Where(i => i.Category == category)
                    .Sum(i => i.AmountCents);
                analyticsWeeks.Add(AnalyticsPeriod.CreateWeek(totalByWeek, currentWeekNumber, request.Date.Year));
            }

            for (
                var currentMonthStartDate = new DateOnly(request.Date.Year, 1, 1);
                currentMonthStartDate < request.Date;
                currentMonthStartDate = currentMonthStartDate.AddMonths(1)
            )
            {
                var totalByMonth = items
                    .Where(i => i.CreatedDate.Month == currentMonthStartDate.Month)
                    .Where(i => i.Category == category)
                    .Sum(i => i.AmountCents);
                analyticsMonths.Add(AnalyticsPeriod.CreateMonth(totalByMonth, currentMonthStartDate.Month, request.Date.Year));
            }

            for (
                var currentQuarterStartDate = new DateOnly(request.Date.Year, 1, 1);
                currentQuarterStartDate < request.Date;
                currentQuarterStartDate = currentQuarterStartDate.AddMonths(3)
            )
            {
                var currentQuarterEndMonth = currentQuarterStartDate.AddMonths(3).Month;
                var totalByQuarter = items
                    .Where(i => i.CreatedDate.Month >= currentQuarterStartDate.Month && i.CreatedDate.Month <= currentQuarterEndMonth)
                    .Where(i => i.Category == category)
                    .Sum(i => i.AmountCents);
                analyticsQuarters.Add(AnalyticsPeriod.CreateQuarter(totalByQuarter, currentQuarterStartDate.Month, request.Date.Year));
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
