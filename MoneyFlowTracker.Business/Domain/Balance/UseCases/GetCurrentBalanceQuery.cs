namespace MoneyFlowTracker.Business.Domain.Balance.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Util.Data;


public class GetCurrentBalanceQueryRequest : IRequest<BalanceModel?>
{
}

public class GetCurrentBalanceQueryRequestHandler(IDataContext dataContext) : IRequestHandler<GetCurrentBalanceQueryRequest, BalanceModel?>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task<BalanceModel?> Handle(GetCurrentBalanceQueryRequest request, CancellationToken cancellationToken)
    {
        return await _dataContext.Balances
            .OrderByDescending(b => b.CreatedDate)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
        ;
    }
}
