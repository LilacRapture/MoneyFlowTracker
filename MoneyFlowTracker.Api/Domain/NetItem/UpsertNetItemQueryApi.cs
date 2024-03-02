namespace MoneyFlowTracker.Api.Domain.NetItem;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.NetItem.UseCases;

public static class UpsertNetItemQueryApi
{
    public class UpsertNetItemRequestDto
    {
        public required Guid Id { get; set; }
        public required Guid CategoryId { get; set; }
        public string? Name { get; set; } = null;
        public required int AmountCents { get; set; }
        public required DateOnly CreatedDate { get; set; }
    }

    public static async Task<IResult> Handler([FromBody] UpsertNetItemRequestDto request, [FromServices] IMediator mediator)
    {
        await mediator.Send(new UpsertNetItemQueryRequest
        {
            Id = request.Id,
            CategoryId = request.CategoryId,
            Name = request.Name,
            AmountCents = request.AmountCents,
            CreatedDate = request.CreatedDate
        });

        return TypedResults.Ok(new object());
    }
}
