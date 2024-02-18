namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public static class GetItemsByDateQueryApi
{
    public static async Task<IResult> Handler([FromRoute] DateTime date, [FromServices] IMediator mediator)
    {
        var items = await mediator.Send(new GetAllItemsQueryRequest
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
