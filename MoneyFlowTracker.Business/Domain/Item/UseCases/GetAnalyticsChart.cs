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
    public static AnalyticsPeriod CreateMonth(int amountCents, DateOnly date)
    {
        var startDate = new DateOnly(date.Year, date.Month, 1);

        return new AnalyticsPeriod
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = startDate.AddMonths(1).AddDays(-1),
        };
    }

    public static AnalyticsPeriod CreateQuartal(int amountCents, DateOnly date)
    {
        var startDate = new DateOnly(date.Year, date.Month, 1);
        startDate = startDate.AddMonths(-3);

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
    IEnumerable<IAnalyticsPeriod> Quartals { get; }
}

public class AnalyticsChart : IAnalyticsChart
{
    public required CategoryModel Category { get; set; }
    public IEnumerable<IAnalyticsPeriod> Weeks { get; set; } = Enumerable.Empty<IAnalyticsPeriod>();
    public IEnumerable<IAnalyticsPeriod> Months { get; set; } = Enumerable.Empty<IAnalyticsPeriod>();
    public IEnumerable<IAnalyticsPeriod> Quartals { get; set; } = Enumerable.Empty<IAnalyticsPeriod>();
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
            for (
                var currentWeekStartDate = new DateOnly(request.Date.Year, 1, 1);
                currentWeekStartDate < request.Date;
                currentWeekStartDate = currentWeekStartDate.AddDays(7)
            )
            {
                var currentWeekNumber = DateHelper.GetWeekOfYear(currentWeekStartDate);
                var total = items
                    .Where(i => DateHelper.GetWeekOfYear(i.CreatedDate) == currentWeekNumber)
                    .Where(i => i.Category == category)
                    .Sum(i => i.AmountCents);
                analyticsWeeks.Add(AnalyticsPeriod.CreateWeek(total, currentWeekNumber, request.Date.Year));
            }
            var analyticsChart = new AnalyticsChart 
            { 
                Category = category,
                Weeks = analyticsWeeks,
            };
            analyticsCharts.Add(analyticsChart);
        }

        return analyticsCharts;
    }
}
