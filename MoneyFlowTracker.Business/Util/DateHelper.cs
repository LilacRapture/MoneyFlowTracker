namespace MoneyFlowTracker.Business.Util;

using System;
using System.Globalization;

public static class DateHelper
{
    public static int GetWeekOfYear(DateOnly date)
    {
        return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
            date.ToDateTime(TimeOnly.MinValue),
            CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday
        );
    }

    public static DateOnly GetWeekDateOnly(int year, int weekNumber)
    {
        var yearStart = new DateOnly(year, 1, 1);
        return yearStart.AddDays((weekNumber - 1) * 7);
    }
}
