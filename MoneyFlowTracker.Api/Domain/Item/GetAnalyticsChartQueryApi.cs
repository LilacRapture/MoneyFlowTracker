namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public class GetAnalyticsChartQueryApi
{
    public static async Task<IResult> Handler([FromServices] IMediator mediator)
    {
        var analyticsChart = await mediator.Send(new GetAnalyticsChartQueryRequest
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
        });
        if (analyticsChart == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(analyticsChart);
    }
}
