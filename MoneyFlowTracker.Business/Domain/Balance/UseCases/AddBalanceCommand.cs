namespace MoneyFlowTracker.Business.Domain.Balance.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Util.Data;


public class UpsertBalanceCommandRequest : IRequest
{
    public required Guid Id { get; set; }
    public required int AmountCents { get; set; }
    public required DateOnly CreatedDate { get; set; }
}

public class UpsertBalanceCommandRequestHandler(IDataContext dataContext) : IRequestHandler<UpsertBalanceCommandRequest>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task Handle(UpsertBalanceCommandRequest request, CancellationToken cancellationToken)
    {
        var currentBalance = await _dataContext.Balances.SingleOrDefaultAsync(
            b => b.CreatedDate == request.CreatedDate,
            cancellationToken: cancellationToken
        );
        if (currentBalance == null)
        {
            var newBalance = new BalanceModel
            {
                Id = request.Id,
                AmountCents = request.AmountCents,
                CreatedDate = request.CreatedDate,
            };
            _dataContext.Balances.Add(newBalance);
        }
        else
        {
            currentBalance.AmountCents = request.AmountCents;
        }

        await _dataContext.SaveChanges(cancellationToken);
    }
}
