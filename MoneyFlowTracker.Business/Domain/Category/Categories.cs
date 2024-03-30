namespace MoneyFlowTracker.Business.Domain.Category;

public class Categories
{
    // Revenue
    public const string Revenue_String = "97d0a0af-0c45-4cbb-b974-519890211827";
    public static Guid Revenue { get; } = Guid.Parse(Revenue_String);

    public const string Floor_String = "f2ee5d5c-6163-4310-b721-11fb518df7fc";
    public static Guid Floor { get; } = Guid.Parse(Floor_String);

    public const string Delivery_String = "a82aad1f-68cb-457e-b1f2-6a6390f51e83";
    public static Guid Delivery { get; } = Guid.Parse(Delivery_String);

    public const string Delivery_Wolt_String = "9731c919-2d40-4384-a181-61134d6bb6c5";
    public static Guid Delivery_Wolt { get; } = Guid.Parse(Delivery_Wolt_String);

    public const string Delivery_Bolt_String = "6b630740-5b37-45d5-900c-08de07098ca3";
    public static Guid Delivery_Bolt { get; } = Guid.Parse(Delivery_Bolt_String);

    public const string Delivery_Glovo_String = "b233490b-a100-466b-987b-49d6da89d58c";
    public static Guid Delivery_Glovo { get; } = Guid.Parse(Delivery_Glovo_String);

    public const string Cash_String = "61fdadaf-2f61-47c3-a139-0020ba652d7d";
    public static Guid Cash { get; } = Guid.Parse(Cash_String);

    public const string Terminal_String = "ee331907-b03e-4c3a-ac1a-c62d56c18390";
    public static Guid Terminal { get; } = Guid.Parse(Terminal_String);

    // Custom Revenue
    public const string CustomRevenue_String = "b1488d86-353a-4e89-a890-de1ecb6ba9bb";
    public static Guid CustomRevenue { get; } = Guid.Parse(CustomRevenue_String);

    public const string CustomRevenue_GrossCash_String = "8bbce42f-f440-4784-8fe1-3c70da523a4e";
    public static Guid CustomRevenue_GrossCash { get; } = Guid.Parse(CustomRevenue_GrossCash_String);

    public const string CustomRevenue_NetTerminal_String = "a7748246-cd7d-4dcb-b217-5a5c100cf0a9";
    public static Guid CustomRevenue_NetTerminal { get; } = Guid.Parse(CustomRevenue_NetTerminal_String);

    public const string CustomRevenue_NetDelivery_String = "e56f662d-696b-47ca-952e-fa2b9271584d";
    public static Guid CustomRevenue_NetDelivery { get; } = Guid.Parse(CustomRevenue_NetDelivery_String);

    // Expenses
    public const string Expenses_String = "b77cbcd2-31df-43bd-80e3-dd0b0fc24184";
    public static Guid Expenses { get; } = Guid.Parse(Expenses_String);

    // Income
    public const string Income_String = "c34656a3-eb18-4357-b7e9-dc9a119cfff3";
    public static Guid Income { get; } = Guid.Parse(Income_String);
}
