namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public static class UpdateItemQueryApi
{
    public class UpdateItemRequestDto
    {
        public required string NewItem { get; set; }
    }

    public static async Task<IResult> Handler([FromBody] UpdateItemRequestDto request, [FromServices] IMediator mediator)
    {
        await mediator.Send(new UpdateItemQueryRequest());
        return TypedResults.Ok();
    }
}
