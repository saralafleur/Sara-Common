using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sara.NETStandard.Common.Extension
{
    public static class DateTimeExt
    {
        public static System.DateTime Tomorrow(this System.DateTime date)
        {
            return date.Date.AddDays(1);
        }
        public static string AbbreviatedTimeZone(this System.DateTime dt, TimeZoneInfo tzi = null)
        {
            tzi = tzi ?? TimeZoneInfo.Local;

            return Regex.Matches(tzi.IsDaylightSavingTime(dt) ? tzi.DaylightName : tzi.StandardName, @"^[A-Z]|\s[A-Z]")
                .OfType<Match>()
                .Select(m => m.Groups[0].Value.Trim())
                .Aggregate((s1, s2) => s1 + s2);
        }
        public static bool IsUnitedStatesTimeZone(this System.DateTime dt, string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
                return false;

            switch (timeZoneId)
            {
                case "Hawaiian Standard Time":
                case "Alaskan Standard Time":
                case "Pacific Standard Time":
                case "Mountain Standard Time":
                case "Central Standard Time":
                case "Eastern Standard Time":
                    return true;
                default:
                    return false;
            }
        }
        public class InvalidDateRangeException : Exception
        {
            public InvalidDateRangeException()
            {

            }
            public InvalidDateRangeException(string message) : base(message)
            {
            }
        }
        public class DateTimeRange
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            public bool Intersects(DateTimeRange test)
            {
                if (Start > End || test.Start > test.End)
                    throw new InvalidDateRangeException();

                if (Start == End || test.Start == test.End)
                    return false; // No actual date range

                if (Start == test.Start || End == test.End)
                    return true; // If any set is the same time, then by default there must be some overlap. 

                if (Start < test.Start)
                {
                    if (End > test.Start && End < test.End)
                        return true; // Condition 1

                    if (End > test.End)
                        return true; // Condition 3
                }
                else
                {
                    if (test.End > Start && test.End < End)
                        return true; // Condition 2

                    if (test.End > End)
                        return true; // Condition 4
                }

                return false;
            }
        }
        public static string DayOfWeekAbr(this DayOfWeek value)
        {
            switch (value)
            {
                case DayOfWeek.Sunday:
                    return "Su";
                case DayOfWeek.Monday:
                    return "M";
                case DayOfWeek.Tuesday:
                    return "Tu";
                case DayOfWeek.Wednesday:
                    return "W";
                case DayOfWeek.Thursday:
                    return "Th";
                case DayOfWeek.Friday:
                    return "F";
                case DayOfWeek.Saturday:
                    return "Sa";
            }
            return "Error";
        }
        public static bool EverySecond(ref DateTime value)
        {
            if ((DateTime.Now - value).TotalSeconds < 1)
                return false;
            value = DateTime.Now;

            return true;
        }
        public const string DATE_FORMAT = "MM/dd/yyyy hh:mm:ss.fff tt";
        public static string ToDayOfWeekString(this DateTime value)
        {
            return value.ToString("ddd MM/dd/yy");
        }
        public static bool TryParseWithTimeZoneRemoval(string value, out DateTime result)
        {
            return DateTime.TryParse(RemoveTimeZone(value), out result);
        }
        private static string RemoveTimeZone(string dateTime)
        {
            return dateTime == null ? "" : dateTime.Substring(0, dateTime.IndexOf("M ", StringComparison.Ordinal) + 1);
        }
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int WeekOfYear(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                time = time.AddDays(3);

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        /// <summary>
        /// Round up to the nearest 15 minute increment
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime TimeRoundUp(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, 0).AddMinutes(input.Minute % 15 == 0 ? 0 : 15 - input.Minute % 15);

        }
        /// <summary>
        /// Round down to the nearest 15 minute increment
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime TimeRoundDown(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, 0).AddMinutes(-input.Minute % 15);
        }

    }
}
