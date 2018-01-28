using System;

namespace BusSched.Services.Helpers
{
    public static class DateTimeExtentions
    {
        public static DateTime TruncateToMinute(this DateTime dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromMinutes(1));
        }

        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}