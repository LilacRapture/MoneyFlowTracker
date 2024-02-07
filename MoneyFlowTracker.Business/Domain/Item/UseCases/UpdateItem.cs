namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class UpdateItemQueryRequest : IRequest
{

}

public class UpdateItemQueryRequestHandler : IRequestHandler<UpdateItemQueryRequest>
{
    public Task Handle(UpdateItemQueryRequest request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
