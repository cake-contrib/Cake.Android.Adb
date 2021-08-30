using System;
using System.IO;
using Cake.AndroidAdb;
using Cake.AndroidAdb.Fakes;
using Xunit;

namespace Cake.Android.Adb.Tests
{
	public class AdbActivityManagerTests : TestFixtureBase
	{
		static string ANDROID_SDK_ROOT
			=> Environment.GetEnvironmentVariable("ANDROID_HOME")
				?? Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT")
				?? File.ReadAllText(Path.Combine(ContentPath, "android_home.txt"))?.Trim();

		static AdbToolSettings GetAdbToolSettings()
		{
			return new AdbToolSettings { SdkRoot = ANDROID_SDK_ROOT };
		}

		[Fact]
		public void Test_StartActivity()
		{
			var r = Cake.AmStartActivity("-a android.settings.SETTINGS", settings: GetAdbToolSettings());

			Assert.True(r);
		}

		[Fact]
		public void Test_StartService()
		{
			var r = Cake.AmStartService("-a android.settings.SETTINGS", settings: GetAdbToolSettings());

			Assert.True(r);
		}

		[Fact]
		public void Test_ForceStop()
		{
			Cake.AmForceStop ("com.android.settings", settings: GetAdbToolSettings());
		}

		[Fact]
		public void Test_Broadcast()
		{
			var r = Cake.AmBroadcast("-a android.settings.SETTINGS", settings: GetAdbToolSettings());
			Assert.True(r == 0);
		}
	}
}
