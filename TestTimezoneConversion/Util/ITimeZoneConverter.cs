using System;

namespace TestTimezoneConversion.Util
{
    public interface ITimeZoneConverter
    {
        DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId);
    }
}