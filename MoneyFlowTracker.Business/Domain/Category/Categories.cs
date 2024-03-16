namespace MoneyFlowTracker.Business.Domain.Category;

public class Categories
{
    // Revenue
    public const string RevenueString = "97d0a0af-0c45-4cbb-b974-519890211827";
    public static Guid Revenue { get; } = Guid.Parse(RevenueString);

    public const string CustomRevenueString = "b1488d86-353a-4e89-a890-de1ecb6ba9bb";
    public static Guid CustomRevenue { get; } = Guid.Parse(CustomRevenueString);

    public const string FloorString = "f2ee5d5c-6163-4310-b721-11fb518df7fc";
    public static Guid Floor { get; } = Guid.Parse(FloorString);

    public const string DeliveryString = "a82aad1f-68cb-457e-b1f2-6a6390f51e83";
    public static Guid Delivery { get; } = Guid.Parse(DeliveryString);

    public const string CashString = "61fdadaf-2f61-47c3-a139-0020ba652d7d";
    public static Guid Cash { get; } = Guid.Parse(CashString);

    public const string TerminalString = "ee331907-b03e-4c3a-ac1a-c62d56c18390";
    public static Guid Terminal { get; } = Guid.Parse(TerminalString);

    // Expenses
    public const string ExpensesString = "b77cbcd2-31df-43bd-80e3-dd0b0fc24184";
    public static Guid Expenses { get; } = Guid.Parse(ExpensesString);

    // Income
    public const string IncomeString = "c34656a3-eb18-4357-b7e9-dc9a119cfff3";
    public static Guid Income { get; } = Guid.Parse(IncomeString);
}
