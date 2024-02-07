namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Util.Data;

public class GetAllItemsQueryRequest : IRequest<ItemModel[]>
{

}

public class GetAllItemsQueryRequestHandler : IRequestHandler<GetAllItemsQueryRequest, ItemModel[]>
{
    private readonly IDataContext _dataContext;
    public GetAllItemsQueryRequestHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<ItemModel[]> Handle(GetAllItemsQueryRequest request, CancellationToken cancellationToken)
    {
        var items = await _dataContext.Items.Include(i => i.Category).ToArrayAsync();
        return items;
    }
}
