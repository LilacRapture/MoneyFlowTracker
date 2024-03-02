namespace MoneyFlowTracker.Api.Domain.NetItem;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.NetItem.UseCases;

public static class GetNetItemQueryApi
{
    public static async Task<IResult> Handler([FromRoute] Guid id, [FromServices] IMediator mediator)
    {
        var item = await mediator.Send(new GetNetItemQueryRequest
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
