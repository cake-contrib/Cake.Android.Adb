using NUnit.Framework;
using System;
using Cake.AndroidAdb.Fakes;
using System.Linq;

namespace Cake.AndroidAdb.Tests
{
	[TestFixture]
	public class CommandsTests : TestFixtureBase
	{

		[Test]
		public void Test_Logcat_empty_log_filter_is_applied_to_output ()
		{
            var r = Cake.AmStartActivity("-a android.settings.SETTINGS", settings: GetAdbToolSettings());
		    var logs = Cake.AdbLogcat(new AdbLogcatOptions(), "*:S", GetAdbToolSettings());
            Assert.AreEqual(2, logs.Where(m => !string.IsNullOrWhiteSpace(m)).ToList().Count);
		}

		[Test]
		public void Test_Logcat_filter_is_applied_to_output ()
		{
            var r = Cake.AmStartActivity("-a android.settings.SETTINGS", settings: GetAdbToolSettings());
		    var logs = Cake.AdbLogcat(new AdbLogcatOptions(), "ActivityManager:I *:S", GetAdbToolSettings());
            Assert.IsTrue(logs.Any(m => m.Contains("I ActivityManager")));
            Assert.IsFalse(logs.Any(m => m.Contains("I AccountManagerService")));
		}

		
    }
}
