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
    public class TimeZoneConverterDotNet : ITimeZoneConverter
    {
        private Dictionary<string, TimeZoneInfo> _timezones;

        public TimeZoneConverterDotNet()
        {
            _timezones = TimeZoneInfo.GetSystemTimeZones().ToDictionary(tzi => tzi.Id);
        }

        public DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
        {
            var sourceTimeZone = _timezones[sourceTimeZoneId];
            var destinationTimeZone = _timezones[destinationTimeZoneId];

            return TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, destinationTimeZone);
        }
    }
}