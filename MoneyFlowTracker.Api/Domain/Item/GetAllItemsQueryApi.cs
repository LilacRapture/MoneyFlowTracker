namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public static class GetAllItemsQueryApi
{
    // TODO: Get items by date
    public static async Task<IResult> Handler([FromServices] IMediator mediator)
    {
        var items = await mediator.Send(new GetAllItemsQueryRequest());
        if (items == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(items);
    }
}
