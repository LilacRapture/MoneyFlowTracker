namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Chart.Services;
using MoneyFlowTracker.Business.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GetAnalyticsChartQueryRequest : IRequest<IEnumerable<IAnalyticsRow>>
{
    public DateOnly Date { get; set; }
}

public class GetAnalyticsChartQueryRequestHandler(
    IDataContext dataContext,
    IAnalyticsChartBuilder analyticsChartBuilder,
    ICustomRevenueService customIncomeService,
    IAnalyticsRowBuilder analyticsRowBuilder
)
  : IRequestHandler<GetAnalyticsChartQueryRequest, IEnumerable<IAnalyticsRow>>
{
    private readonly IDataContext _dataContext = dataContext;
    private readonly IAnalyticsChartBuilder _analyticsChartBuilder = analyticsChartBuilder;
    private readonly ICustomRevenueService _customIncomeService = customIncomeService;
    private readonly IAnalyticsRowBuilder _analyticsRowBuilder = analyticsRowBuilder;

    public async Task<IEnumerable<IAnalyticsRow>> Handle(GetAnalyticsChartQueryRequest request, CancellationToken cancellationToken)
    {
        var items = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == request.Date.Year)
            .ToListAsync(cancellationToken: cancellationToken)
        ;

        var categories = await _dataContext.Category.ToListAsync(cancellationToken: cancellationToken);
        var grossAnalyticsCharts = _analyticsChartBuilder.Build(items, categories, request.Date);
        var customRevenueCharts = await _customIncomeService.CreateCustomRevenueCharts(request.Date);
        var incomeChart = CreateIncomeChart(
            customRevenueCharts.Single(c => c.Category.Id == Categories.CustomRevenue), 
            grossAnalyticsCharts.Single(c => c.Category.Id == Categories.Expenses)
        );

        var allAnalyticsCharts = grossAnalyticsCharts
            .Concat(customRevenueCharts)
            .Concat([incomeChart]);
        var analyticsRows = _analyticsRowBuilder.Build(allAnalyticsCharts);

        return analyticsRows;
    }

    private static IAnalyticsChart CreateIncomeChart(IAnalyticsChart revenue, IAnalyticsChart expense)
    {
        var incomeWeeks = revenue.Weeks.Zip(expense.Weeks, (rp, ep) => new AnalyticsChartPoint
        {
            AmountCents = rp.AmountCents - ep.AmountCents,
            StartDate = rp.StartDate,
            EndDate = rp.EndDate,
        });
        var incomeMonths = revenue.Months.Zip(expense.Months, (rp, ep) => new AnalyticsChartPoint
        {
            AmountCents = rp.AmountCents - ep.AmountCents,
            StartDate = rp.StartDate,
            EndDate = rp.EndDate,
        });
        var incomeQuarters = revenue.Quarters.Zip(expense.Quarters, (rp, ep) => new AnalyticsChartPoint
        {
            AmountCents = rp.AmountCents - ep.AmountCents,
            StartDate = rp.StartDate,
            EndDate = rp.EndDate,
        });

        return new AnalyticsChart
        {
            Category = new CategoryModel
            {
                Id = Categories.Income,
                Name = "Прибыль",
                IsIncome = true,
            },
            Weeks = incomeWeeks,
            Months = incomeMonths,
            Quarters = incomeQuarters,
        };
    }
}
