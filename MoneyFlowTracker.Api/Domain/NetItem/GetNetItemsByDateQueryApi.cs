namespace MoneyFlowTracker.Api.Domain.NetItem;

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.NetItem.UseCases;

public static class GetNetItemsByDateQueryApi
{
    public class NetItemReponseDto
    {
        public Guid Id { get; set; }
        public int AmountCents { get; set; }
        public string? Name { get; set; } = null;
        public bool IsNet { get; set; }
        public DateOnly CreatedDate { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryModel Category { get; set; } = null!;
    }

    public static async Task<IResult> Handler(
        [FromRoute] DateTime date,
        [FromServices] IMapper mapper,
        [FromServices] IMediator mediator
    ) {
        var items = await mediator.Send(new GetNetItemsByDateQueryRequest
        {
            Date = DateOnly.FromDateTime(date),
        });
        if (items == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(
            mapper.Map<NetItemReponseDto[]>(items)
        );
    }
}
