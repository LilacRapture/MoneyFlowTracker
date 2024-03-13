namespace MoneyFlowTracker.Business.Domain.Chart.UseCases;

using MediatR;
using Microsoft.EntityFrameworkCore;
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
    IAnalyticsRowBuilder analyticsRowBuilder
)
  : IRequestHandler<GetAnalyticsChartQueryRequest, IEnumerable<IAnalyticsRow>>
{
    private readonly IDataContext _dataContext = dataContext;
    private readonly IAnalyticsChartBuilder _analyticsChartBuilder = analyticsChartBuilder;
    private readonly IAnalyticsRowBuilder _analyticsRowBuilder = analyticsRowBuilder;

    public async Task<IEnumerable<IAnalyticsRow>> Handle(GetAnalyticsChartQueryRequest request, CancellationToken cancellationToken)
    {
        var items = await _dataContext.Items
            .Include(i => i.Category)
            .Where(item => item.CreatedDate.Year == request.Date.Year)
            .ToListAsync(cancellationToken: cancellationToken)
        ;

        var categories = await _dataContext.Category.ToListAsync(cancellationToken: cancellationToken);
        var analyticsCharts = _analyticsChartBuilder.Build(items, categories, request.Date);
        var analyticsRows = _analyticsRowBuilder.Build(analyticsCharts);

        return analyticsRows;
    }
}
