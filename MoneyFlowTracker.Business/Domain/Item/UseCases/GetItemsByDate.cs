namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Util.Data;

public class GetItemsByDateQueryRequest : IRequest<ItemModel[]>
{
    public required DateOnly Date { get; set; }
}

public class GetItemsByDateQueryRequestHandler(IDataContext dataContext) : IRequestHandler<GetItemsByDateQueryRequest, ItemModel[]>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task<ItemModel[]> Handle(GetItemsByDateQueryRequest request, CancellationToken cancellationToken)
    {
        return await _dataContext.Items
            .Include(i => i.Category)
            .Where(i => i.CreatedDate == request.Date)
            .ToArrayAsync(cancellationToken: cancellationToken)
        ;
    }
}
