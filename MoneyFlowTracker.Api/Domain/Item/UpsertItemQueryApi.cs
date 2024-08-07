﻿namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public static class UpsertItemQueryApi
{
    public class UpsertItemRequestDto
    {
        public required Guid Id { get; set; }
        public required Guid CategoryId { get; set; }
        public string? Name { get; set; } = null;
        public required int AmountCents { get; set; }
        public required DateOnly CreatedDate { get; set; }
    }

    public static async Task<IResult> Handler([FromBody] UpsertItemRequestDto request, [FromServices] IMediator mediator)
    {
        await mediator.Send(new UpsertItemQueryRequest
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
