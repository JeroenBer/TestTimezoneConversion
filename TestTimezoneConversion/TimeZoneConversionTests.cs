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

namespace TestTimezoneConversion
{
    internal static class TimeZoneConversionTests
    {
        private static ITimeZoneConverter _converter = new TimeZoneConverterDotNet();

        public static void TestConversion()
        {
            var inputDateTime = new DateTime(2023, 5, 9, 12, 15, 30, DateTimeKind.Unspecified);

            var resultDateTime = _converter.Convert(inputDateTime, "UTC", "Europe/Amsterdam");

            var expectedDateTime = new DateTime(2023, 5, 9, 14, 15, 30, DateTimeKind.Unspecified);

            resultDateTime.Should().Be(expectedDateTime);
            resultDateTime.Kind.Should().Be(expectedDateTime.Kind);
        }
    }
}