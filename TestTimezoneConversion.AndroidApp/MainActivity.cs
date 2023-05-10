using Android.OS;
using TestTimezoneConversion.AndroidApp.Tests;

namespace TestTimezoneConversion.AndroidApp
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

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
            var sw = new System.Diagnostics.Stopwatch();
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

    }
}