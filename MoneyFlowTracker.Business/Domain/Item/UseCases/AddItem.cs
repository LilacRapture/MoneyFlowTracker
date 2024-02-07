namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFlowTracker.Business.Util.Data;

public class AddItemQueryRequest : IRequest
{
    public required Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
    public required int AmountCents { get; set; }
    public required DateTime CreatedDate { get; set; }
}

public class AddItemQueryRequestHandler : IRequestHandler<AddItemQueryRequest>
{
    private readonly IDataContext _dataContext;
    public AddItemQueryRequestHandler(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task Handle(AddItemQueryRequest request, CancellationToken cancellationToken)
    {
        var newItem = new ItemModel
        {
            Id = request.Id,
            CategoryId = request.CategoryId,
            AmountCents = request.AmountCents,
            CreatedDate = request.CreatedDate,
        };
        _dataContext.Items.Add(newItem);
        await _dataContext.SaveChanges(cancellationToken);
    }
}
