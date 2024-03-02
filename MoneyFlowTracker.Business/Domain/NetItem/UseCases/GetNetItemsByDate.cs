namespace MoneyFlowTracker.Business.Domain.NetItem.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Util.Data;

public class GetNetItemsByDateQueryRequest : IRequest<NetItemModel[]>
{
    public required DateOnly Date { get; set; }
}

public class GetNetItemsByDateQueryRequestHandler(IDataContext dataContext) : IRequestHandler<GetNetItemsByDateQueryRequest, NetItemModel[]>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task<NetItemModel[]> Handle(GetNetItemsByDateQueryRequest request, CancellationToken cancellationToken)
    {
        return await _dataContext.NetItems
            .Include(i => i.Category)
            .Where(i => i.CreatedDate == request.Date)
            .ToArrayAsync(cancellationToken: cancellationToken)
        ;
    }
}
