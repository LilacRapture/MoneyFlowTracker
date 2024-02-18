namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public static class UpdateItemQueryApi
{
    public class UpdateItemRequestDto
    {
        public required Guid Id { get; set; }
        public required int AmountCents { get; set; }
    }

    public static async Task<IResult> Handler([FromBody] UpdateItemRequestDto request, [FromServices] IMediator mediator)
    {
        await mediator.Send(new UpdateItemQueryRequest
        {
            Id = request.Id,
            AmountCents = request.AmountCents,
        });

        return TypedResults.Ok(new object());
    }
}
