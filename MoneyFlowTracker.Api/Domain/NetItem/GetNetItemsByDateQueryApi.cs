namespace MoneyFlowTracker.Api.Domain.NetItem;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.NetItem.UseCases;

public static class GetNetItemsByDateQueryApi
{
    public static async Task<IResult> Handler([FromRoute] DateTime date, [FromServices] IMediator mediator)
    {
        var items = await mediator.Send(new GetNetItemsByDateQueryRequest
        {
            Date = DateOnly.FromDateTime(date),
        });
        if (items == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(items);
    }
}
