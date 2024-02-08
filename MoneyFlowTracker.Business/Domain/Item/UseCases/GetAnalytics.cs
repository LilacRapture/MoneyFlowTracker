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
    int AmountTodayCents { get; set; }
    int WeekChangePercent { get; set; }
    int MonthChangePercent { get; set; }
    int QuarterChangePercent { get; set; }
}

public class AnalyticsRow : IAnalyticsRow
{
    public required CategoryModel Category { get; set; }
    public int AmountTodayCents { get; set; }
    public int WeekChangePercent { get; set; }
    public int MonthChangePercent { get; set; }
    public int QuarterChangePercent { get; set; }
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
        DateTime todayStartDate = DateTime.SpecifyKind(request.Date.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
        DateTime todayEndDate = DateTime.SpecifyKind(request.Date.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc);
        var itemsGrouppedByCategory = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate >= todayStartDate &&
                           item.CreatedDate <= todayEndDate)
            .GroupBy(i => i.Category)
            .ToArrayAsync();
        var analyticsRows = itemsGrouppedByCategory
            .ToDictionary(g => g.Key, g => g.Sum(i => i.AmountCents))
            .Select(kvp => new AnalyticsRow { 
                Category = kvp.Key,
                AmountTodayCents = kvp.Value,
            });

        return analyticsRows;
    }
}
