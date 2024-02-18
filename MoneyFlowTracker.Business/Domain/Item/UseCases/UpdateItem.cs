namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFlowTracker.Business.Util.Data;

public class UpdateItemQueryRequest : IRequest
{
    public required Guid Id { get; set; }
    public required int AmountCents { get; set; }
}

public class UpdateItemQueryRequestHandler(IDataContext dataContext) : IRequestHandler<UpdateItemQueryRequest>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task Handle(UpdateItemQueryRequest request, CancellationToken cancellationToken)
    {
        await _dataContext.Items
            .Where(i => i.Id == request.Id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(i => i.AmountCents, request.AmountCents),
                cancellationToken: cancellationToken
            )
        ;

        await _dataContext.SaveChanges(cancellationToken);
    }
}
