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

namespace TestTimezoneConversion.AndroidApp.Converters
{
    public class TimeZoneConverterAndroidOffset : ITimeZoneConverter
    {
        private object _lock = new object();

        private Dictionary<string, Java.Util.TimeZone> _timezones = new Dictionary<string, Java.Util.TimeZone>();
        private Dictionary<string, Java.Util.Calendar> _calendars = new Dictionary<string, Java.Util.Calendar>();


        public DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
        {
            lock (_lock) // Not sure if it's thread safe, just in case
            {
                var androidSourceTimeZone = GetCachedTimeZone(sourceTimeZoneId);
                var androidDestinationTimeZone = GetCachedTimeZone(destinationTimeZoneId);

                var calendar = GetCalendar(sourceTimeZoneId);
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

        private Java.Util.TimeZone GetCachedTimeZone(string id)
        {
            if (!_timezones.ContainsKey(id))
            {
                _timezones.Add(id, Java.Util.TimeZone.GetTimeZone(id));
            }
            return _timezones[id];
        }

        private Java.Util.Calendar GetCalendar(string id)
        {
            if (!_calendars.ContainsKey(id))
            {
                var tz = GetCachedTimeZone(id);
                _calendars.Add(id, Java.Util.Calendar.GetInstance(tz));
            }
            return _calendars[id];
        }


    }
}