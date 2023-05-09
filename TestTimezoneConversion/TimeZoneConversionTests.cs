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
using TestTimezoneConversion.Util;

namespace TestTimezoneConversion
{
    internal static class TimeZoneConversionTests
    {

        private static void TestConversion(ITimeZoneConverter timeZoneConverter)
        {
            var inputDateTime = new DateTime(2023, 5, 9, 12, 15, 30, DateTimeKind.Unspecified);

            var resultDateTime = timeZoneConverter.Convert(inputDateTime, "UTC", "Europe/Amsterdam");

            var expectedDateTime = new DateTime(2023, 5, 9, 14, 15, 30, DateTimeKind.Unspecified);

            resultDateTime.Should().Be(expectedDateTime);
            resultDateTime.Kind.Should().Be(expectedDateTime.Kind);
        }

        public static void TestConversionDotNet()
        {
            TestConversion(new TimeZoneConverterDotNet());
        }

        public static void TestConversionJava()
        {
            TestConversion(new TimeZoneConverterJava());
        }

        public static void TestConversionAndroidIcu()
        {
            TestConversion(new TimeZoneConverterAndroidIcu());
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