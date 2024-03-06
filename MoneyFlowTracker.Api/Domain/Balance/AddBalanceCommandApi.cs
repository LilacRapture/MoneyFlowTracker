namespace MoneyFlowTracker.Api.Domain.Balance;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Balance.UseCases;


public static class AddBalanceCommandApi
{
    public class AddBalanceCommandRequestDto
    {
        public required int AmountCents { get; set; }
        public required DateOnly CreatedDate { get; set; }
    }

    public static async Task<IResult> Handler([FromBody] AddBalanceCommandRequestDto request, [FromServices] IMediator mediator)
    {
        var newBalanceId = Guid.NewGuid();

        await mediator.Send(new UpsertBalanceCommandRequest
        {
            Id = newBalanceId,
            AmountCents = request.AmountCents,
            CreatedDate = request.CreatedDate
        });


        var responseBody = new
        {
            Id = newBalanceId,
        };
        return TypedResults.Ok(responseBody);
    }
}
