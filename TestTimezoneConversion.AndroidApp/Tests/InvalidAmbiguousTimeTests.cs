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

namespace TestTimezoneConversion.AndroidApp.Tests
{
    public class InvalidAmbiguousTimeTests
    {
        public static void TestInvalidTime()
        {
            var amsterdamTimeZone = TimeZoneInfo.GetSystemTimeZones().Single(tzi => tzi.Id == "Europe/Amsterdam");

            var isInvalidTime = amsterdamTimeZone.IsInvalidTime(new DateTime(2023, 3, 26, 2, 30, 0));
            isInvalidTime.Should().BeTrue();
        }

        public static void TestAmbiguousTime()
        {
            var amsterdamTimeZone = TimeZoneInfo.GetSystemTimeZones().Single(tzi => tzi.Id == "Europe/Amsterdam");

            var isAmbiguousTime = amsterdamTimeZone.IsAmbiguousTime(new DateTime(2023, 10, 29, 2, 30, 0));
            isAmbiguousTime.Should().BeTrue();
        }

    }
}