﻿using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace TestTimezoneConversion
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            var btnStartTests = FindViewById<Android.Widget.Button>(Resource.Id.btnStartTests);
            btnStartTests.Click += BtnStartTests_Click;
        }

        private async void BtnStartTests_Click(object sender, EventArgs e)
        {
            await RunTest(nameof(TimeZoneConversionTests.TestConversionDotNet), TimeZoneConversionTests.TestConversionDotNet);
            await RunTest(nameof(TimeZoneConversionTests.TestConversionSimpleDateFormat), TimeZoneConversionTests.TestConversionSimpleDateFormat);
            await RunTest(nameof(TimeZoneConversionTests.TestConversionAndroidOffset), TimeZoneConversionTests.TestConversionAndroidOffset);

            await RunTest(nameof(InvalidAmbiguousTimeTests.TestInvalidTime), InvalidAmbiguousTimeTests.TestInvalidTime);
            await RunTest(nameof(InvalidAmbiguousTimeTests.TestAmbiguousTime), InvalidAmbiguousTimeTests.TestAmbiguousTime);

            await RunPerformanceTest(nameof(TimeZoneConverterPerformanceTests.TestPerformanceDotNet), TimeZoneConverterPerformanceTests.TestPerformanceDotNet);
            await RunPerformanceTest(nameof(TimeZoneConverterPerformanceTests.TestPerformanceAndroidOffset), TimeZoneConverterPerformanceTests.TestPerformanceAndroidOffset);
            await RunPerformanceTest(nameof(TimeZoneConverterPerformanceTests.TestPerformanceSimpleDateFormat), TimeZoneConverterPerformanceTests.TestPerformanceSimpleDateFormat);

            if ((int)Build.VERSION.SdkInt <= 33 && Build.VERSION.Codename != "UpsideDownCake")
            {
                // Android 14 or higher ?
                // We cannot run this test 
                await RunTest(nameof(TimeZoneConverterAndroidOffsetTests), TimeZoneConverterAndroidOffsetTests.TestAndroidOffsetConversion);
            }
        }

        private async Task RunTest(string testName, Action executeTest)
        {
            try
            {
                executeTest();

                await ShowMessage($"{testName} results", "Test succesfull");
            }
            catch (Exception ex)
            {
                await ShowMessage($"{testName} results", $"Error in test: {ex}");
            }
        }

        private async Task RunPerformanceTest(string testName, Action executeTest)
        {
            var sw = new Stopwatch();
            sw.Start();
            executeTest();
            sw.Stop();

            await ShowMessage($"{testName} results", $"Performance {sw.Elapsed.TotalSeconds}.{sw.Elapsed.Milliseconds} sec");
        }

        private async Task ShowMessage(string title, string message)
        {
            var wait = new SemaphoreSlim(0);
            var dialog = new Android.App.AlertDialog.Builder(this);
            var alert = dialog.Create();
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetButton("OK", (c, ev) =>
            {
                wait.Release(1);
            });
            alert.Show();
            await wait.WaitAsync();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
