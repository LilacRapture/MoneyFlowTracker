namespace MoneyFlowTracker.Business.Domain.Category;

using MoneyFlowTracker.Business.Domain.Item;
using System.Text.Json.Serialization;


public class CategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsIncome {  get; set; }

    public Guid? ParentCategoryId { get; set; } = null;
    public CategoryModel? ParentCategory { get; set; } = null;
    [JsonIgnore]
    public ICollection<CategoryModel> ChildCategories { get; } = [];

    [JsonIgnore]
    public ICollection<ItemModel> Items { get; } = [];
}
