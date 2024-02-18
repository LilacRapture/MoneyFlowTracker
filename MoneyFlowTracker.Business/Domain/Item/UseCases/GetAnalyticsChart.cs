namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Util;
using MoneyFlowTracker.Business.Util.Data;
using System;
using System.Collections.Generic;
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
        var itemsByCategory = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == request.Date.Year)
            .GroupBy(i => i.Category)
            .ToDictionaryAsync(g => g.Key, g => g.ToList())
        ;

        // TODO: Take into account sub-categories within a category,
        // e.g. total of ЗП should include totals of ЗП повар, ЗП бар, etc
        var analyticsChart = itemsByCategory.Select(kvp => new AnalyticsChart {
            Category = kvp.Key,
            Weeks = kvp.Value.GroupBy(
                i => DateHelper.GetWeekOfYear(i.CreatedDate),
                (k, g) => AnalyticsPeriod.CreateWeek(g.Sum(i => i.AmountCents), k, request.Date.Year)
            ),
            Months = kvp.Value.GroupBy(
                i => i.CreatedDate.Month,
                (k, g) => AnalyticsPeriod.CreateMonth(g.Sum(i => i.AmountCents), request.Date)
            ),
            Quartals = kvp.Value.GroupBy(
                i => i.CreatedDate.Month,
                (k, g) => AnalyticsPeriod.CreateQuartal(g.Sum(i => i.AmountCents), request.Date)
            ),
        });

        return analyticsChart; 
    }
}
