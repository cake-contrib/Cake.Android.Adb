using System;
using Cake.AndroidAdb;
using Cake.AndroidAdb.Fakes;
using NUnit.Framework;

namespace Cake.Android.Adb.Tests
{
	[TestFixture]
	public class AdbActivityManagerTests : TestFixtureBase
	{
		const string SDK_ROOT = "../../../android-sdk";

		AdbToolSettings GetAdbToolSettings()
		{
			return new AdbToolSettings { SdkRoot = SDK_ROOT };
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
