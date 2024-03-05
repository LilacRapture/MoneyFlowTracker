namespace MoneyFlowTracker.Business.Domain.Balance.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFlowTracker.Business.Util.Data;


public class AddBalanceCommandRequest : IRequest
{
    public required Guid Id { get; set; }
    public required int AmountCents { get; set; }
    public required DateOnly CreatedDate { get; set; }
}

public class AddBalanceCommandRequestHandler(IDataContext dataContext) : IRequestHandler<AddBalanceCommandRequest>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task Handle(AddBalanceCommandRequest request, CancellationToken cancellationToken)
    {
        var newBalance = new BalanceModel
        {
            Id = request.Id,
            AmountCents = request.AmountCents,
            CreatedDate = request.CreatedDate,
        };
        _dataContext.Balances.Add(newBalance);

        await _dataContext.SaveChanges(cancellationToken);
    }
}
