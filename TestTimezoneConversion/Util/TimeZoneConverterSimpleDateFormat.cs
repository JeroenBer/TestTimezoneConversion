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
        public DateTime Convert(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
        {
            // Conversion this way works but is very inefficient and might not work if separators differ for locale
            // Only for API level 24 and higher

            const string JavaDateFormat = "yyyyMMddHHmmss";
            const string NetDateFormat = "yyyyMMddHHmmss";

            if (dateTime.Year <= 1) return dateTime;

            var androidSourceTimeZone = Android.Icu.Util.TimeZone.GetTimeZone(sourceTimeZoneId);
            var androidDestinationTimeZone = Android.Icu.Util.TimeZone.GetTimeZone(destinationTimeZoneId);

            var sourceFormat = new Android.Icu.Text.SimpleDateFormat(JavaDateFormat);
            sourceFormat.TimeZone = androidSourceTimeZone;
            var parsedDt = sourceFormat.Parse(dateTime.ToString(NetDateFormat));

            var destFormat = new Android.Icu.Text.SimpleDateFormat(JavaDateFormat);
            destFormat.TimeZone = androidDestinationTimeZone;
            var resultDtString = destFormat.Format(parsedDt);

            var result = DateTime.ParseExact(resultDtString, NetDateFormat, null, System.Globalization.DateTimeStyles.None);

            //Debug.WriteLine($"ConvertTime FROM {dateTime:yyyy-MM-dd HH:mm:ss} {sourceTimeZoneId} -> {result:yyyy-MM-dd HH:mm:ss} {destinationTimeZoneId}");

            return result;
        }
    }
}