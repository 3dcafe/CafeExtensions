namespace CafeExtensions;
/// <summary>
/// Extension for work with DateTime of Type
/// </summary>
public static class DateTimeExtension
{
    public static DateTime StartOfDay(this DateTime theDate)
    {
        return theDate.Date;
    }

    public static DateTime EndOfDay(this DateTime theDate)
    {
        return theDate.Date.AddDays(1).AddTicks(-1);
    }

    public static long ToUnixTimestamp(this DateTime d)
    {
        var epoch = d - new DateTime(1970, 1, 1, 0, 0, 0);
        return (long)epoch.TotalSeconds;
    }
    /// <summary>
    /// Convert to unix time
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
    /// does the date interval intersect in another date interval
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

    public static DateTime? SetKindUtc(this DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return dateTime.Value.SetKindUtc();
        }
        else
        {
            return null;
        }
    }
    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc) { return dateTime; }
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}