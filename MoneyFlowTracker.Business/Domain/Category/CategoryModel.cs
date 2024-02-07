using MoneyFlowTracker.Business.Domain.Item;
using System.Text.Json.Serialization;

namespace MoneyFlowTracker.Business.Domain.Category;

public class CategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsIncome {  get; set; }

    [JsonIgnore]
    public ICollection<ItemModel> Items { get; } = [];
}
