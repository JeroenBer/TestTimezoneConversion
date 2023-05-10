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
    public class TimeZoneConverterSimpleDateFormat : ITimeZoneConverter
    {
        const string JavaDateFormat = "yyyyMMddHHmmss";
        const string NetDateFormat = "yyyyMMddHHmmss";

        private object _lock = new object();

        private Dictionary<string, Android.Icu.Util.TimeZone> _timezones = new Dictionary<string, Android.Icu.Util.TimeZone>();

        private Android.Icu.Text.SimpleDateFormat formatter = new Android.Icu.Text.SimpleDateFormat(JavaDateFormat);

        public DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
        {
            // Conversion this way works but is very inefficient
            // Only for API level 24 and higher

            lock (_lock) // Not sure if it's thread safe, just in case
            {
                if (dateTime.Year <= 1) return dateTime;

                var androidSourceTimeZone = GetCachedTimeZone(sourceTimeZoneId);
                var androidDestinationTimeZone = GetCachedTimeZone(destinationTimeZoneId);

                formatter.TimeZone = androidSourceTimeZone;
                var parsedDt = formatter.Parse(dateTime.ToString(NetDateFormat));

                formatter.TimeZone = androidDestinationTimeZone;
                var resultDtString = formatter.Format(parsedDt);

                var result = DateTime.ParseExact(resultDtString, NetDateFormat, null, System.Globalization.DateTimeStyles.None);

                return result;
            }
        }

        private Android.Icu.Util.TimeZone GetCachedTimeZone(string id)
        {
            if (!_timezones.ContainsKey(id))
            {
                _timezones.Add(id, Android.Icu.Util.TimeZone.GetTimeZone(id));
            }
            return _timezones[id];
        }
    }
}