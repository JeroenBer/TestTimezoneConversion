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
using System.Threading;
using TestTimezoneConversion.AndroidApp.Converters;

namespace TestTimezoneConversion.AndroidApp.Tests
{
    internal static class TimeZoneConversionTests
    {

        private static void TestConversion(ITimeZoneConverter timeZoneConverter)
        {
            TestConversion(timeZoneConverter, new DateTime(2023, 5, 9, 12, 15, 30, DateTimeKind.Unspecified), "UTC", "Europe/Amsterdam", new DateTime(2023, 5, 9, 14, 15, 30, DateTimeKind.Unspecified));
            TestConversion(timeZoneConverter, new DateTime(2023, 5, 9, 14, 15, 30, DateTimeKind.Unspecified), "Europe/Amsterdam", "UTC", new DateTime(2023, 5, 9, 12, 15, 30, DateTimeKind.Unspecified));

            TestConversion(timeZoneConverter, new DateTime(2023, 5, 9, 12, 15, 30, DateTimeKind.Unspecified), "UTC", "America/New_York", new DateTime(2023, 5, 9, 8, 15, 30, DateTimeKind.Unspecified));
            TestConversion(timeZoneConverter, new DateTime(2023, 5, 9, 14, 15, 30, DateTimeKind.Unspecified), "America/New_York", "UTC", new DateTime(2023, 5, 9, 18, 15, 30, DateTimeKind.Unspecified));
        }

        private static void TestConversion(ITimeZoneConverter timeZoneConverter, DateTime inputDateTime, string sourceTimeZoneId, string destinationTimeZoneId, DateTime expectedDateTime)
        {
            var resultDateTime = timeZoneConverter.Convert(inputDateTime, sourceTimeZoneId, destinationTimeZoneId);

            resultDateTime.Should().Be(expectedDateTime);
            resultDateTime.Kind.Should().Be(expectedDateTime.Kind);
        }


        public static void TestConversionDotNet()
        {
            TestConversion(new TimeZoneConverterDotNet());
        }

        public static void TestConversionSimpleDateFormat()
        {
            TestConversion(new TimeZoneConverterSimpleDateFormat());
        }

        public static void TestConversionAndroidOffset()
        {
            TestConversion(new TimeZoneConverterAndroidOffset());
        }

    }
}