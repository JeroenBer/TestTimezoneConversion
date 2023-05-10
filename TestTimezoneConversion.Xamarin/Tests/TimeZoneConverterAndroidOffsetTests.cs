using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTimezoneConversion.Util;

namespace TestTimezoneConversion
{
    internal class TimeZoneConverterAndroidOffsetTests
    {
        private static TimeZoneConverterDotNet _converterDotNet = new TimeZoneConverterDotNet();
        private static ITimeZoneConverter _converterAndroidOffset = new TimeZoneConverterAndroidOffset();
        
        public static void TestAndroidOffsetConversion()
        {
            var allTimezones = TimeZoneInfo.GetSystemTimeZones();
            var deviations = new List<string>();

            foreach(var timezone in allTimezones)
            {
                deviations.AddRange(TestAndroidOffsetConversion(timezone.Id, "America/New_York"));
                deviations.AddRange(TestAndroidOffsetConversion("America/New_York", timezone.Id));
            }

            if (deviations.Count > 0)
            {
                throw new Exception("Deviations in Android converion:" + String.Join(System.Environment.NewLine, deviations));
            }
        }

        private static List<string> TestAndroidOffsetConversion(string sourceTimeZone, string destinationTimeZone)
        {
            var deviations = new List<string>();

            // Test 
            var inputDateTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

            while (inputDateTime.Year == 2023)
            {
                var tzi = TimeZoneInfo.GetSystemTimeZones().Single(tzi => tzi.Id == sourceTimeZone);

                if (!tzi.IsInvalidTime(inputDateTime))
                {
                    var resultDateTime = _converterAndroidOffset.Convert(inputDateTime, sourceTimeZone, destinationTimeZone);

                    var expectedDateTime = _converterDotNet.Convert(inputDateTime, sourceTimeZone, destinationTimeZone);

                    if (resultDateTime != expectedDateTime)
                    {
                        deviations.Add($"{sourceTimeZone} -> {destinationTimeZone}: {resultDateTime:yyyy-MM-dd HH:mm:ss}, expected {expectedDateTime:yyyy-MM-dd HH:mm:ss}");
                        return deviations;
                    }
                }

                inputDateTime = inputDateTime.AddMinutes(15);
            }

            return deviations;
        }
    }
}