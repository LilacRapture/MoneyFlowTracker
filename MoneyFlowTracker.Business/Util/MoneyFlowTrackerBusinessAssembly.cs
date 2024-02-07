namespace MoneyFlowTracker.Business.Util;

using System.Reflection;


public static class MoneyFlowTrackerBusinessAssembly
{
    public static Assembly GetAssembly()
    {
        return typeof(MoneyFlowTrackerBusinessAssembly).Assembly;
    }
}
