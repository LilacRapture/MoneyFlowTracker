namespace MoneyFlowTracker.Business.Domain.Chart;

using MoneyFlowTracker.Business.Util;


public class AnalyticsChartPoint : IAnalyticsChartPoint
{
    public int AmountCents { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public static AnalyticsChartPoint CreateWeek(int amountCents, int weekNumber, int year)
    {
        var startDate = DateHelper.GetWeekDateOnly(year, weekNumber);

        return new AnalyticsChartPoint
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = startDate.AddDays(6),
        }; 
    }
    public static AnalyticsChartPoint CreateMonth(int amountCents, int month, int year)
    {
        var startDate = new DateOnly(year, month, 1);

        return new AnalyticsChartPoint
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = startDate.AddMonths(1).AddDays(-1),
        };
    }

    public static AnalyticsChartPoint Create(int amountCents, DateOnly startDate, DateOnly endDate)
    {
        return new AnalyticsChartPoint
        {
            AmountCents = amountCents,
            StartDate = startDate,
            EndDate = endDate,
        };
    }
}
