using System;

namespace TestTimezoneConversion
{
    public interface ITimeZoneConverter
    {
        DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId);
    }
}