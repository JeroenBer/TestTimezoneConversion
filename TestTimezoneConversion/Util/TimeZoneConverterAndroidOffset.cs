using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestTimezoneConversion.Util
{
    public class TimeZoneConverterAndroidOffset : ITimeZoneConverter
    {
        public DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
        {
            var androidSourceTimeZone = Java.Util.TimeZone.GetTimeZone(sourceTimeZoneId);
            var androidDestinationTimeZone = Java.Util.TimeZone.GetTimeZone(destinationTimeZoneId);

            var calendar = Java.Util.Calendar.GetInstance(androidSourceTimeZone);
            calendar.Set(dateTime.Year, dateTime.Month - 1, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            var timeInMs = calendar.TimeInMillis;

            // Dirty trick, might not always work correctly ?
            // See https://stackoverflow.com/questions/6088778/converting-utc-dates-to-other-timezones (3rd option)
            var fromOffset = androidSourceTimeZone.GetOffset(timeInMs);
            var toOffset = androidDestinationTimeZone.GetOffset(timeInMs);

            var diff = fromOffset - toOffset;

            var result = dateTime.AddMilliseconds(diff * -1).WithKind(DateTimeKind.Unspecified);

            return result;
        }
    }
}