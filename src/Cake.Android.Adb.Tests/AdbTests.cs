using Xunit;
using Cake.AndroidAdb.Fakes;
using System.Linq;
using System;
using System.IO;

namespace Cake.AndroidAdb.Tests
{
	public class AdbTests : TestFixtureBase
	{
		static string ANDROID_SDK_ROOT
			=> Environment.GetEnvironmentVariable("ANDROID_HOME")
				?? File.ReadAllText(System.IO.Path.Combine(ContentPath, "android_home.txt"))?.Trim();

		AdbToolSettings GetAdbToolSettings()
		{
			return new AdbToolSettings { SdkRoot = ANDROID_SDK_ROOT };
		}

		[Fact]
		public void Test_Adb_Devices()
		{
			var devices = Cake.AdbDevices(GetAdbToolSettings());

			Assert.NotNull(devices);
		}

		//[Fact]
		public void Test_Adb_WaitForEmulator()
		{
			var s = GetAdbToolSettings();

			s.Serial = "emulator-5576";

			var booted = Cake.AdbWaitForEmulatorToBoot(TimeSpan.FromMinutes(2), s);

			Assert.True(booted);
		}
	}
}
