namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Util.Data;

public class GetAnalyticsQueryRequest : IRequest<IEnumerable<AnalyticsRow>>
{
    public DateOnly Date { get; set; }
}

public interface IAnalyticsRow
{
    CategoryModel Category { get; set; }
    int? AmountTodayCents { get; set; }
    int? WeekChangePercent { get; set; }
    int? MonthChangePercent { get; set; }
    int? QuarterChangePercent { get; set; }
}

public class AnalyticsRow : IAnalyticsRow
{
    public required CategoryModel Category { get; set; }
    public int? AmountTodayCents { get; set; } = null;
    public int? WeekChangePercent { get; set; } = null;
    public int? MonthChangePercent { get; set; } = null;
    public int? QuarterChangePercent { get; set; } = null;
}

public class GetAnalyticsQueryRequestHandler : IRequestHandler<GetAnalyticsQueryRequest, IEnumerable<AnalyticsRow>>
{
    private readonly IDataContext _dataContext;
    public GetAnalyticsQueryRequestHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<IEnumerable<AnalyticsRow>> Handle(GetAnalyticsQueryRequest request, CancellationToken cancellationToken)
    {
        var itemsByCategoryToday = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate == request.Date)
            .GroupBy(i => i.Category)
            .ToDictionaryAsync(g => g.Key, g => g.Sum(i => i.AmountCents));

        var itemsByCategoryWeekAgo = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate == request.Date.AddDays(-7))
            .GroupBy(i => i.Category)
            .ToDictionaryAsync(g => g.Key, g => g.Sum(i => i.AmountCents));

        var itemsByCategoryMonthAgo = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate == request.Date.AddMonths(-1))
            .GroupBy(i => i.Category)
            .ToDictionaryAsync(g => g.Key, g => g.Sum(i => i.AmountCents));

        var itemsByCategory3MonthsAgo = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate == request.Date.AddMonths(-3))
            .GroupBy(i => i.Category)
            .ToDictionaryAsync(g => g.Key, g => g.Sum(i => i.AmountCents));

        var categories = await _dataContext.Category.ToArrayAsync();
        var analyticsRows = new List<AnalyticsRow>();

        foreach (var category in categories)
        {
            if (itemsByCategoryToday.ContainsKey(category) && itemsByCategoryToday[category] != 0)
            {
                var amountCentsToday = itemsByCategoryToday[category];
                var amountWeekAgo = itemsByCategoryWeekAgo.ContainsKey(category) ? itemsByCategoryWeekAgo[category] : 0;
                var weekChangePercent = (amountWeekAgo - amountCentsToday) / amountCentsToday * 100;

                var amountMonthAgo = itemsByCategoryMonthAgo.ContainsKey(category) ? itemsByCategoryMonthAgo[category] : 0;
                var monthChangePercent = (amountMonthAgo - amountCentsToday) / amountCentsToday * 100;

                var amount3MonthsAgo = itemsByCategory3MonthsAgo.ContainsKey(category) ? itemsByCategory3MonthsAgo[category] : 0;
                var quarterChangePercent = (amount3MonthsAgo - amountCentsToday) / amountCentsToday * 100;
                var analyticsRow = new AnalyticsRow
                {
                    Category = category,
                    AmountTodayCents = amountCentsToday,
                    WeekChangePercent = weekChangePercent,
                    MonthChangePercent = monthChangePercent,
                    QuarterChangePercent = quarterChangePercent,
                };
                analyticsRows.Add(analyticsRow);
            }
            else
            {
                var analyticsRow = new AnalyticsRow
                {
                    Category = category,
                };
            }
        }

        return analyticsRows;
    }
}
