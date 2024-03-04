namespace MoneyFlowTracker.Business.Domain.Chart;


public interface IAnalyticsPeriod
{
    int AmountCents { get; }
    DateOnly StartDate { get; }
    DateOnly EndDate { get; }
}
