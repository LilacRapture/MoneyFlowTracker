namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public static class GetAnalyticsQueryApi
{
    public static async Task<IResult> Handler([FromRoute] DateOnly date, [FromServices] IMediator mediator)
    {
        var analyticsRows = await mediator.Send(new GetAnalyticsQueryRequest
        {
            Date = date,
        });
        if (analyticsRows == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(analyticsRows);
    }
}
