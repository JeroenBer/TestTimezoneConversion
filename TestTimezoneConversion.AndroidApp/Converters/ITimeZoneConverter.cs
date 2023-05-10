using System;

namespace TestTimezoneConversion.AndroidApp.Converters
{
    public interface ITimeZoneConverter
    {
        DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId);
    }
}