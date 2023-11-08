using System.Globalization;

namespace CafeExtensions.Extensions;
/// <summary>
/// Extension for work with DateTime of Type
/// </summary>
public static class DateTimeExtension
{
    /// <summary>
    /// Return Date time text Russian format
    /// </summary>
    /// <param name="date">дата</param>
    /// <param name="isLastLogin"></param>
    /// <returns></returns>
    public static string ToStringTime(DateTimeOffset date, bool isLastLogin)
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        //if(IsSocket==false)
        //    date = date.AddHours(-3);

        var timeSpan = DateTimeOffset.Now.Subtract(date);
        //new TimeSpan(DateTime.Now - date);
        double delta = Math.Abs(timeSpan.TotalSeconds);
        //Для пользователей
        if (isLastLogin == true)
        {
            string template = "Был(а) ";
            string templateBack = " назад";
            if (delta < 60 * MINUTE)
            {
                int minute = Math.Abs(timeSpan.Minutes);
                if (minute < 1)
                    return template + "только что";
                if (minute >= 11 && minute <= 19)
                    return template + minute + " минут" + templateBack;
                var i = minute % 10;
                switch (i)
                {
                    case 1:
                        return template + minute + " минуту" + templateBack;
                    case 2:
                    case 3:
                    case 4:
                        return template + minute + " минуты" + templateBack;
                    default:
                        return template + minute + " минут" + templateBack;
                }
            }
            if (delta < 24 * HOUR)
            {
                if (timeSpan.Hours >= 11 && timeSpan.Hours <= 19)
                    return template + timeSpan.Hours + " часов" + templateBack;
                var i = timeSpan.Hours % 10;
                switch (i)
                {
                    case 1:
                        return template + timeSpan.Hours + " час" + templateBack;
                    case 2:
                    case 3:
                    case 4:
                        return template + timeSpan.Hours + " часа" + templateBack;
                    default:
                        return template + timeSpan.Hours + " часов" + templateBack;
                }
            }

            if (delta < 30 * DAY)
            {
                if (timeSpan.Days < 7)
                {
                    var i = timeSpan.Days % 10;
                    switch (i)
                    {
                        case 1:
                            return template + timeSpan.Days + " день" + templateBack;
                        case 5:
                        case 6:
                            return template + timeSpan.Days + " дней" + templateBack;
                        default:
                            return template + timeSpan.Days + " дня" + templateBack;
                    }
                }
                if (timeSpan.Days >= 7 && timeSpan.Days < 14)
                    return template + " неделю" + templateBack;
                if (timeSpan.Days >= 14 && timeSpan.Days < 21)
                    return template + "2 недели" + templateBack;
                if (timeSpan.Days >= 21 && timeSpan.Days < 28)
                    return template + "3 недели" + templateBack;
                else
                    return template + "4 недели" + templateBack;
            }

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30));
                if (months > 1 && months < 5)
                    return template + months + " месяца" + templateBack;
                if (months >= 5 && months < 12)
                    return template + months + " месяцев" + templateBack;
                else
                    return template + "1 месяц" + templateBack;
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365));
                int number = years;
                if (years > 20)
                    number = number % 10;
                switch (number)
                {
                    case 1:
                        return template + years + " год" + templateBack;
                    case 2:
                    case 3:
                    case 4:
                        return template + years + " года" + templateBack;
                    default:
                        return template + years + " лет" + templateBack;
                }
            }
        }
        //Для сообщений
        else
        {
            //DateTime dateTime = date;
            if (delta < 24 * HOUR)
                return date.ToString("HH:mm");
            if (delta < 30 * DAY)
                if (timeSpan.Days < 7)
                {
                    string dayWeek = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(date.DayOfWeek);
                    char[] array = dayWeek.ToCharArray();
                    string dayWeekShort = "";
                    if (dayWeek == "вторник" || dayWeek == "среда")
                        return String.Join(dayWeekShort, array[0], array[1]);
                    return String.Join(dayWeekShort, array[0], array[2]);
                }
            if (delta < 12 * MONTH)
                return date.ToString("dd.MM");
            else
                return date.ToString("dd.MM.yy");
        }
    }
    /// <summary>
    /// Return date in text format ru region
    /// </summary>
    /// <param name="date">дата</param>
    /// <param name="isLastLogin"></param>
    /// <returns></returns>
    public static string ToStringTime(long date, bool isLastLogin)
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        var timeSpan = new TimeSpan(DateTime.Now.Ticks - date);
        double delta = Math.Abs(timeSpan.TotalSeconds);
        //Для пользователей
        if (isLastLogin == true)
        {
            string template = "Был(а) ";
            string templateBack = " назад";
            if (delta < 60 * MINUTE)
            {
                int minute = Math.Abs(timeSpan.Minutes);
                if (minute < 1)
                    return template + "только что";
                if (minute >= 11 && minute <= 19)
                    return template + minute + " минут" + templateBack;
                var i = minute % 10;
                switch (i)
                {
                    case 1:
                        return template + minute + " минуту" + templateBack;
                    case 2:
                    case 3:
                    case 4:
                        return template + minute + " минуты" + templateBack;
                    default:
                        return template + minute + " минут" + templateBack;
                }
            }
            if (delta < 24 * HOUR)
            {
                if (timeSpan.Hours >= 11 && timeSpan.Hours <= 19)
                    return template + timeSpan.Hours + " часов" + templateBack;
                var i = timeSpan.Hours % 10;
                switch (i)
                {
                    case 1:
                        return template + timeSpan.Hours + " час" + templateBack;
                    case 2:
                    case 3:
                    case 4:
                        return template + timeSpan.Hours + " часа" + templateBack;
                    default:
                        return template + timeSpan.Hours + " часов" + templateBack;
                }
            }

            if (delta < 30 * DAY)
            {
                if (timeSpan.Days < 7)
                {
                    var i = timeSpan.Days % 10;
                    switch (i)
                    {
                        case 1:
                            return template + timeSpan.Days + " день" + templateBack;
                        case 5:
                        case 6:
                            return template + timeSpan.Days + " дней" + templateBack;
                        default:
                            return template + timeSpan.Days + " дня" + templateBack;
                    }
                }
                if (timeSpan.Days >= 7 && timeSpan.Days < 14)
                    return template + " неделю" + templateBack;
                if (timeSpan.Days >= 14 && timeSpan.Days < 21)
                    return template + "2 недели" + templateBack;
                if (timeSpan.Days >= 21 && timeSpan.Days < 28)
                    return template + "3 недели" + templateBack;
                else
                    return template + "4 недели" + templateBack;
            }

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30));
                if (months > 1 && months < 5)
                    return template + months + " месяца" + templateBack;
                if (months >= 5 && months < 12)
                    return template + months + " месяцев" + templateBack;
                else
                    return template + "1 месяц" + templateBack;
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365));
                int number = years;
                if (years > 20)
                    number = number % 10;
                switch (number)
                {
                    case 1:
                        return template + years + " год" + templateBack;
                    case 2:
                    case 3:
                    case 4:
                        return template + years + " года" + templateBack;
                    default:
                        return template + years + " лет" + templateBack;
                }
            }
        }
        //Для сообщений
        else
        {
            DateTime dateTime = new DateTime(date);
            if (delta < 24 * HOUR)
                return dateTime.ToString("HH:mm");
            if (delta < 30 * DAY)
                if (timeSpan.Days < 7)
                {
                    string dayWeek = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(dateTime.DayOfWeek);
                    char[] array = dayWeek.ToCharArray();
                    string dayWeekShort = "";
                    if (dayWeek == "вторник" || dayWeek == "среда")
                        return String.Join(dayWeekShort, array[0], array[1]);
                    return String.Join(dayWeekShort, array[0], array[2]);
                }
            if (delta < 12 * MONTH)
                return dateTime.ToString("dd.MM");
            else
                return dateTime.ToString("dd.MM.yy");
        }
    }
    /// <summary>
    /// This method returns a new DateTime object representing the start of the day for the given date. 
    /// It uses the Date property to zero out the hours, minutes, seconds, and milliseconds of the time, leaving only the date portion.
    /// This is useful if you need to perform operations related to the start of a day.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime StartOfDay(this DateTime dateTime)
    {
        return dateTime.Date;
    }
    /// <summary>
    /// This method returns a new DateTime object representing the end of the day for the given date. 
    /// It starts by obtaining the start of the next day, adds one second (in ticks), and then subtracts 1 tick. 
    /// This method can be useful when you need an exact representation of the end of the day.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime EndOfDay(this DateTime dateTime)
    {
        return dateTime.Date.AddDays(1).AddTicks(-1);
    }
    /// <summary>
    /// This method converts a DateTime object to a Unix timestamp (the number of seconds elapsed since January 1, 1970). 
    /// It subtracts the initial date (January 1, 1970) from the provided date and returns the result as a long integer.
    /// Note that this method does not account for time zones.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToUnixTimestamp(this DateTime dateTime)
    {
        var epoch = dateTime - new DateTime(1970, 1, 1, 0, 0, 0);
        return (long)epoch.TotalSeconds;
    }
    /// <summary>
    /// Convert to unix time
    /// This method is designed to convert a Unix timestamp (representing the number of seconds since January 1, 1970) into a DateTime object.
    /// It does so by first creating a base DateTime object set to the Unix epoch (January 1, 1970, 00:00:00 UTC).
    /// Then, it adds the provided number of seconds (the Unix timestamp) to this base date and finally converts the resulting DateTime object to the local time.
    /// </summary>
    /// <param name="unixtime"></param>
    /// <returns></returns>
    public static DateTime UnixTimeToDateTime(this long unixtime)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
        return dtDateTime;
    }
    /// <summary>
    /// Does the date interval intersect in another date interval
    /// </summary>
    /// <param name="start1"></param>
    /// <param name="end1"></param>
    /// <param name="start2"></param>
    /// <param name="end2"></param>
    /// <returns></returns>
    public static bool IsIntersect(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
    {
        return start1 < end2 && start2 < end1;
    }
    /// <summary>
    /// Add local time
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static DateTime AddLocalTimeHours(this DateTime d)
    {
        return d.AddHours(3);
    }
    /// <summary>
    /// The method serves a specific purpose, ensuring that a DateTime object is in the Utc kind, which can be useful when dealing with time zone-agnostic operations or when you need consistency in your application's handling of time.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc) { return dateTime; }
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}