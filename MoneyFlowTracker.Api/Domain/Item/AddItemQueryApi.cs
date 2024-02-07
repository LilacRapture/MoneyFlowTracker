﻿namespace MoneyFlowTracker.Api.Domain.Item;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Item.UseCases;

public static class AddItemQueryApi
{
    public class AddItemRequestDto
    {
        public required Guid Id { get; set; }
        public required Guid CategoryId { get; set; }
        public required int AmountCents { get; set; }
        public required DateTime CreatedDate { get; set; }
    }

    public static async Task<IResult> Handler([FromBody] AddItemRequestDto request, [FromServices] IMediator mediator)
    {
        await mediator.Send(new AddItemQueryRequest
        { 
            Id = request.Id,
            CategoryId = request.CategoryId,
            AmountCents = request.AmountCents,
            CreatedDate = request.CreatedDate
        });
        return TypedResults.Ok();
    }
}