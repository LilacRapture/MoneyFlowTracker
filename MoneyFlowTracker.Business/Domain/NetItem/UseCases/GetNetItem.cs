namespace MoneyFlowTracker.Business.Domain.NetItem.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Util.Data;

public class GetNetItemQueryRequest : IRequest<NetItemModel?>
{
    public Guid Id { get; set; }
}

public class GetNetItemQueryRequestHandler(IDataContext dataContext) : IRequestHandler<GetNetItemQueryRequest, NetItemModel?>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task<NetItemModel?> Handle(GetNetItemQueryRequest request, CancellationToken cancellationToken)
    {
        return await _dataContext.NetItems
            .Include(i => i.Category)
            .SingleOrDefaultAsync(
                i => i.Id == request.Id,
                cancellationToken: cancellationToken
            )
        ;
    }
}
