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
    public class TimeZoneConverterAndroidIcu : ITimeZoneConverter
    {
        public DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
        {
            var androidSourceTimeZone = Android.Icu.Util.TimeZone.GetTimeZone(sourceTimeZoneId);
            var androidDestinationTimeZone = Android.Icu.Util.TimeZone.GetTimeZone(destinationTimeZoneId);

            var calendar = Android.Icu.Util.Calendar.GetInstance(androidSourceTimeZone);
            calendar.TimeZone = androidSourceTimeZone;
            calendar.Set(dateTime.Year, dateTime.Month - 1, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            calendar.TimeZone = androidDestinationTimeZone;

            var t = calendar.Time;

            var result = new DateTime(t.Year + 1900, t.Month + 1, t.Day + 1, t.Hours, t.Minutes, t.Seconds, DateTimeKind.Unspecified);

            return result;
        }
    }
}