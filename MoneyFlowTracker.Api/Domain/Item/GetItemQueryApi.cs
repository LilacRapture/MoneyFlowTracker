namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public static class GetItemQueryApi
{
    public static async Task<IResult> Handler([FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        var item = await mediator.Send(new GetItemQueryRequest
        {
            Id = id,
        });
        if (item == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(item);
    }
}
