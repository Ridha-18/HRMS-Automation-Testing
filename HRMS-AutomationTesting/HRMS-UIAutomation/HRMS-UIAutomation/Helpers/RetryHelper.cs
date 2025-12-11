using System;
using System.Diagnostics;
using System.Threading;

namespace HRMS_UIAutomation.Helpers;

public class Retry
{
    public static void Do(Action action,
                    TimeSpan? interval = null,
                    int retries = 3)
    {
        if (interval == null) { interval = TimeSpan.FromSeconds(1); }
        Try<object, Exception>(() => {
            action();
            return null;
        }, interval, retries);
    }

    public static T Do<T>(Func<T> action, Predicate<T> predicate)
    {
        var exceptions = new Exception();
        Stopwatch sw = new Stopwatch();
        sw.Start();
        while (sw.ElapsedMilliseconds < 30000)
        {
            try
            {
                bool succeeded;
                T result;
                do
                {
                    result = action();
                    succeeded = predicate(result);
                } while (!succeeded && sw.ElapsedMilliseconds < 30000);

                return result;
            }
            catch (Exception ex)
            {
                exceptions = ex;
            }
        }

        throw new Exception(exceptions.ToString());
    }

    private static T Try<T, E>(Func<T> action,
                       TimeSpan? interval,
                       int retries = 3) where E : Exception
    {
        var exceptions = new Exception();

        for (int retry = 0; retry < retries; retry++)
        {
            try
            {
                if (retry > 0)
                    Thread.Sleep(interval.Value);
                return action();
            }
            catch (E ex)
            {
                exceptions = ex;
            }
        }

        throw new Exception(exceptions.ToString());
    }
}
