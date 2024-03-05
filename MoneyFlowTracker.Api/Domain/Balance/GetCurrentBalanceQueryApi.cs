namespace MoneyFlowTracker.Api.Domain.Balance;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Balance.UseCases;

public static class GetCurrentBalanceQueryApi
{
    public static async Task<IResult> Handler([FromServices] IMediator mediator)
    {
        var currentBalance = await mediator.Send(new GetCurrentBalanceQueryRequest());
        if (currentBalance == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(currentBalance);
    }
}
