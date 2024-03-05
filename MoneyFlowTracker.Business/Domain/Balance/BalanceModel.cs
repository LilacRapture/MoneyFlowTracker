namespace MoneyFlowTracker.Business.Domain.Balance;

using System;


public class BalanceModel
{
    public Guid Id { get; set; }
    public int AmountCents { get; set; }
    public DateOnly CreatedDate { get; set; }
}
