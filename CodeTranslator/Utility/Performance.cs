using System;

namespace CodeTranslator.Utility
{
    public static class Performance
    {
        public static TimeSpan GetTimeElapsed(Action action)
        {
            var startTime = DateTime.Now;
            action.Invoke();
            return DateTime.Now - startTime;
        }
    }
}
