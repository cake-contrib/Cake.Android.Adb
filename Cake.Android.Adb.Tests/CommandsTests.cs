using NUnit.Framework;
using System;
using Cake.AndroidAdb.Fakes;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cake.AndroidAdb.Tests
{
    [TestFixture]
    public class CommandsTests : TestFixtureBase
    {
        public sealed class TestResults
        {
            public int Run { get; set; }
            public int Passed { get; set; }
            public int Failed { get; set; }
            public int Skipped { get; set; }
            public int Inconclusive { get; set; }
        }


        [Test]
        public void Test_Logcat_empty_log_filter_is_applied_to_output()
        {
            var r = Cake.AmStartActivity("-a android.settings.SETTINGS", settings: GetAdbToolSettings());
            var logs = Cake.AdbLogcat(new AdbLogcatOptions(), "*:S", GetAdbToolSettings());
            Assert.AreEqual(3, logs.Where(m => !string.IsNullOrWhiteSpace(m)).ToList().Count);
        }

        [Test]
        public void Test_Logcat_filter_is_applied_to_output()
        {
            var r = Cake.AmStartActivity("-a android.settings.SETTINGS", settings: GetAdbToolSettings());
            var logs = Cake.AdbLogcat(new AdbLogcatOptions(), "ActivityManager:I *:S", GetAdbToolSettings());
            Assert.IsTrue(logs.Any(m => m.Contains("I ActivityManager")));
            Assert.IsFalse(logs.Any(m => m.Contains("I AccountManagerService")));
        }

        [Test]
        public void Test_Logcat_filter_is_applied_to_output_and_can_parse_tests()
        {
            var packageId = "com.bluechilli.chillisource.mobile.tests";
			Cake.AdbShell(string.Format("am start -n {0}/{1} -c android.intent.category.LAUNCHER", packageId, "com.xunit.runneractivity"), GetAdbToolSettings());
            var logs = Cake.AdbLogcat(new AdbLogcatOptions(), "mono-stdout:I *:S", GetAdbToolSettings());
            var results = GetTestResultsFromLogs(logs);
            Assert.IsTrue(logs.Any(m => m.Contains("I mono-stdout")));
            Assert.AreEqual(3, results.Passed);
		}

        private TestResults GetTestResultsFromLogs(List<string> logItems)
        {
            {
                var testResults = new TestResults();
                logItems.Reverse();

                foreach (var line in logItems)
                {
                    // Unit for Devices = "Tests run: 0 Passed: 0 Failed: 0 Skipped: 0"
                    // NUnit for Devices = "Tests run: 2 Passed: 1 Inconclusive: 0 Failed: 1 Ignored: 1
                    if (line.Contains("Tests run:"))
                    {
                        var testLine = line.Substring(line.IndexOf("Tests run:", StringComparison.Ordinal));
                        var testArray = Regex.Split(testLine, @"\D+").Where(s => s != string.Empty).ToArray();
                        testResults.Run = int.Parse(testArray[0]);
                        testResults.Passed = int.Parse(testArray[1]);
                        if (testArray.Length == 4)
                        {
                            testResults.Failed = int.Parse(testArray[2]);
                            testResults.Skipped = int.Parse(testArray[3]);
                        }
                        else
                        {
                            testResults.Inconclusive = int.Parse(testArray[2]);
                            testResults.Failed = int.Parse(testArray[3]);
                            testResults.Skipped = int.Parse(testArray[4]);
                        }
                        break;
                    }
                }

                return testResults;
            }
        }
    }
}
