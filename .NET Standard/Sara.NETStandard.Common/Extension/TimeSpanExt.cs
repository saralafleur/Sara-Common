using System;

namespace Sara.NETStandard.Common.Extension
{
    public static class TimeSpanExt
    {
        public static string ToExcelString(this int value)
        {
            return value == 0 ? "" : value.ToString();
        }
        public static string ToExcelString(this TimeSpan value)
        {
            return value.Ticks == 0 ? "" : value.ToString();
        }
        public static string ToExcelString(this bool value)
        {
            return value ? "x" : "";
        }
        public static string ToFixedTimeString(this TimeSpan span)
        {
            return span.ToString("hh':'mm':'ss'.'fff");
        }
        public static TimeSpan Difference(this DateTime start, DateTime stop)
        {
            return stop - start;
        }
        public static string ToShorterReadableString(this TimeSpan span, int padRight = 0)
        {
            var formatted =
                $"{(span.Duration().Days > 0 ? $"{span.Days:0} d, " : string.Empty)}{(span.Duration().Hours > 0 ? $"{span.Hours:0} h, " : string.Empty)}{(span.Duration().Minutes > 0 ? $"{span.Minutes:0} m, " : string.Empty)}";

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 m";

            if (padRight - formatted.Length > 0)
                formatted = formatted + new string(' ', padRight - formatted.Length);

            return formatted;
        }
        public static string ToCleanString(this TimeSpan span)
        {
            var test = span.Duration();

            var result = $"{test.Days:0}";

            if (test.Days > 0)
                result += ":";

            result = test.Days > 0 ? $"{test.Hours:00}" : $"{test.Hours:#0}";

            if (test.Days > 0 || test.Hours > 0)
                result += ":";

            result = $"{test.Minutes:#0}:{test.Seconds:00}";

            if (test.Milliseconds > 0)
                result += $".{test.Milliseconds:000}";

            return result;
        }
        public static string ToShortReadableString(this TimeSpan span, int padRight = 0)
        {
            var formatted =
                $"{(span.Duration().Days > 0 ? $"{span.Days:0} d, " : string.Empty)}{(span.Duration().Hours > 0 ? $"{span.Hours:0} h, " : string.Empty)}{(span.Duration().Minutes > 0 ? $"{span.Minutes:0} m, " : string.Empty)}{(span.Duration().Seconds > 0 ? $"{span.Seconds.ToString().PadLeft(2)} s, " : string.Empty)}{(span.Duration().Milliseconds > 0 ? $"{span.Milliseconds:0} ms" : string.Empty)}";

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 s";

            if (padRight - formatted.Length > 0)
                formatted = formatted + new string(' ', padRight - formatted.Length);

            return formatted;
        }
        public static string ToReadableString(this TimeSpan span, int padRight = 0)
        {
            var formatted =
                $"{(span.Duration().Days > 0 ? $"{span.Days:0} day{(span.Days == 1 ? String.Empty : "s")}, " : string.Empty)}{(span.Duration().Hours > 0 ? $"{span.Hours:0} hour{(span.Hours == 1 ? String.Empty : "s")}, " : string.Empty)}{(span.Duration().Minutes > 0 ? $"{span.Minutes:0} minute{(span.Minutes == 1 ? String.Empty : "s")}, " : string.Empty)}{(span.Duration().Seconds > 0 ? $"{span.Seconds:0} second{(span.Seconds == 1 ? String.Empty : "s")}, " : string.Empty)}{(span.Duration().Milliseconds > 0 ? $"{span.Milliseconds:0} milliseconds" : string.Empty)}";

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

            if (padRight - formatted.Length > 0)
                formatted += new string(' ', padRight - formatted.Length);

            return formatted;
        }
        public static string FormatTimeSpan(TimeSpan value)
        {
            if (value.TotalMilliseconds < 1000)
                return $"{Math.Round(value.TotalMilliseconds, 2)} ms";

            if (value.TotalSeconds < 60)
                return $"{Math.Round(value.TotalSeconds, 2)} sec {Math.Round(value.TotalMilliseconds % 1000, 2)} ms";

            return
                $"{Math.Round(value.TotalMinutes, 2)} min {Math.Round(value.TotalSeconds % 60, 2)} sec {Math.Round(value.TotalMilliseconds % 1000, 2)} ms";
        }
    }
}
