namespace MoneyFlowTracker.Business.Domain.Item.UseCases;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFlowTracker.Business.Util.Data;

public class UpsertItemQueryRequest : IRequest
{
    public required Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
    public string? Name { get; set; } = null;
    public required int AmountCents { get; set; }
    public required DateOnly CreatedDate { get; set; }
}

public class UpsertItemQueryRequestHandler(IDataContext dataContext) : IRequestHandler<UpsertItemQueryRequest>
{
    private readonly IDataContext _dataContext = dataContext;

    public async Task Handle(UpsertItemQueryRequest request, CancellationToken cancellationToken)
    {
        var item = await _dataContext.Items.FindAsync(
            [request.Id],
            cancellationToken: cancellationToken
        );
        if (item == null)
        {
            var newItem = new ItemModel
            {
                Id = request.Id,
                CategoryId = request.CategoryId,
                Name = request.Name,
                AmountCents = request.AmountCents,
                CreatedDate = request.CreatedDate,
            };
            _dataContext.Items.Add(newItem);
        }
        else
        {
            item.CategoryId = request.CategoryId;
            item.Name = request.Name;
            item.AmountCents = request.AmountCents;
            item.CreatedDate = request.CreatedDate;
        }

        await _dataContext.SaveChanges(cancellationToken);
    }
}
