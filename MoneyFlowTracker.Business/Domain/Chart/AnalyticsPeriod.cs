namespace MoneyFlowTracker.Business.Domain.Chart;

using MoneyFlowTracker.Business.Util;


public class AnalyticsPeriod : IAnalyticsPeriod
{
    public int AmountCents { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public static AnalyticsPeriod CreateWeek(int amountCents, int weekNumber, int year)
    {
        var startDate = DateHelper.GetWeekDateOnly(year, weekNumber);

        return new AnalyticsPeriod
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = startDate.AddDays(6),
        }; 
    }
    public static AnalyticsPeriod CreateMonth(int amountCents, int month, int year)
    {
        var startDate = new DateOnly(year, month, 1);

        return new AnalyticsPeriod
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = startDate.AddMonths(1).AddDays(-1),
        };
    }

    public static AnalyticsPeriod Create(int amountCents, DateOnly startDate, DateOnly endDate)
    {
        return new AnalyticsPeriod
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = endDate,
        };
    }
}
