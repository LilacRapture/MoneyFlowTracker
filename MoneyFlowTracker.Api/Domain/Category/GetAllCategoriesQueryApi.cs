namespace MoneyFlowTracker.Api.Domain.Category;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoneyFlowTracker.Business.Domain.Categories.UseCases;

public static class GetAllCategoriesQueryApi
{
    public static async Task<IResult> Handler([FromServices] IMediator mediator)
    {
        var categories = await mediator.Send(new GetAllCategoriesQueryRequest());
        if (categories == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(categories);
    }
}
