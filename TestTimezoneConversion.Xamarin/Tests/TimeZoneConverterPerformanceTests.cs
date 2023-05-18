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
    internal class TimeZoneConverterPerformanceTests
    {
        public static void TestGetSystemTimeZones()
        {
            var lst = TimeZoneInfo.GetSystemTimeZones();
            System.Diagnostics.Debug.WriteLine($"GetSystemTimeZones {lst.Count}");
        }

        public static void TestPerformanceDotNet()
            => TestPerformance(new TimeZoneConverterDotNet());

        public static void TestPerformanceAndroidOffset()
            => TestPerformance(new TimeZoneConverterAndroidOffset());

        public static void TestPerformanceSimpleDateFormat()
            => TestPerformance(new TimeZoneConverterSimpleDateFormat());

        private static void TestPerformance(ITimeZoneConverter timeZoneConverter)
        {
            var inputDateTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

            while (inputDateTime.Year == 2023)
            {
                var tzi = TimeZoneInfo.GetSystemTimeZones().Single(tzi => tzi.Id == "America/New_York");

                if (!tzi.IsInvalidTime(inputDateTime))
                {
                    var resultDateTime = timeZoneConverter.Convert(inputDateTime, "America/New_York", "Europe/Amsterdam");
                }

                inputDateTime = inputDateTime.AddMinutes(15);
            }
        }
    }
}