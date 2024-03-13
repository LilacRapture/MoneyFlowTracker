namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Chart.UseCases;

public class GetAnalyticsChartQueryApi
{
    // TODO: In case of an exception response with 500 with an instance of ErrorResponse class in the body
    public static async Task<IResult> Handler([FromServices] IMediator mediator)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.Now);

        var analyticsRows = await mediator.Send(new GetAnalyticsChartQueryRequest
        {
            Date = dateNow,
        });
        if (analyticsRows == null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(analyticsRows);
    }
}
