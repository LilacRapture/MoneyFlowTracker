namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Util.Data;

public class GetItemQueryRequest : IRequest<ItemModel?>
{
    public Guid Id { get; set; }
}

public class GetItemQueryRequestHandler : IRequestHandler<GetItemQueryRequest, ItemModel?>
{
    private readonly IDataContext _dataContext;
    public GetItemQueryRequestHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<ItemModel?> Handle(GetItemQueryRequest request, CancellationToken cancellationToken)
    {
        var item = await _dataContext.Items.Include(i => i.Category).SingleOrDefaultAsync(i => i.Id == request.Id);
        return item;
    }
}
