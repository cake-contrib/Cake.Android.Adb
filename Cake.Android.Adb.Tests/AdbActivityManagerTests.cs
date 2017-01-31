using System;
using Cake.AndroidAdb;
using Cake.AndroidAdb.Fakes;
using NUnit.Framework;

namespace Cake.Android.Adb.Tests
{
	[TestFixture]
	public class AdbActivityManagerTests : TestFixtureBase
	{
		const string TOOL_PATH = "../../../android-sdk/platform-tools/adb";

		AdbToolSettings GetAdbToolSettings()
		{
			return new AdbToolSettings { ToolPath = TOOL_PATH };
		}

		[Test]
		public void Test_StartActivity()
		{
			var r = Cake.AmStartActivity("-a android.settings.SETTINGS", settings: GetAdbToolSettings());

			Assert.IsTrue(r);
		}

		[Test]
		public void Test_StartService()
		{
			var r = Cake.AmStartService("-a android.settings.SETTINGS", settings: GetAdbToolSettings());

			Assert.IsTrue(r);
		}

		[Test]
		public void Test_ForceStop()
		{
			Cake.AmForceStop ("com.android.settings", settings: GetAdbToolSettings());
		}

		[Test]
		public void Test_Broadcast()
		{
			var r = Cake.AmBroadcast("-a android.settings.SETTINGS", settings: GetAdbToolSettings());
			Assert.IsTrue(r == 0);
		}
	}
}
