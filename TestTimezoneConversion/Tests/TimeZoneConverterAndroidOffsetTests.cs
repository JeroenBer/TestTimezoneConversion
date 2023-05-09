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
        public static void TestAndroidOffsetConversion()
        {
            var converterDotNet = new TimeZoneConverterDotNet();
            var converterOffset = new TimeZoneConverterAndroidOffset();

            // Test 
            var inputDateTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

            while (inputDateTime.Year == 2023)
            {
                var tzi = TimeZoneInfo.GetSystemTimeZones().Single(tzi => tzi.Id == "America/New_York");

                if (!tzi.IsInvalidTime(inputDateTime))
                {
                    var resultDateTime = converterOffset.Convert(inputDateTime, "America/New_York", "Europe/Amsterdam");

                    var expectedDateTime = converterDotNet.Convert(inputDateTime, "America/New_York", "Europe/Amsterdam");

                    resultDateTime.Should().Be(expectedDateTime);
                    resultDateTime.Kind.Should().Be(expectedDateTime.Kind);
                }

                inputDateTime = inputDateTime.AddMinutes(15);
            }
        }
    }
}