namespace MoneyFlowTracker.Business.Domain.Categories.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Util.Data;

public class GetAllCategoriesQueryRequest : IRequest<CategoryModel[]>
{
}

public class GetAllCategoriesQueryRequestHandler : IRequestHandler<GetAllCategoriesQueryRequest, CategoryModel[]>
{
    private readonly IDataContext _dataContext;
    public GetAllCategoriesQueryRequestHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<CategoryModel[]> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
    {
        return await _dataContext.Category.ToArrayAsync(cancellationToken: cancellationToken);
    }
}
