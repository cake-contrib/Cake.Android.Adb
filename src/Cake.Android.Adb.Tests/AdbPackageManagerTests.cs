using Xunit;
using Cake.AndroidAdb.Fakes;
using System.Linq;
using System;
using System.IO;

namespace Cake.AndroidAdb.Tests
{
	public class AdbPackageManagerTests : TestFixtureBase
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
		public void Test_List_Permissions()
		{
			var permissionGroups = Cake.PmListPermissions(settings: GetAdbToolSettings());

			Assert.NotEmpty(permissionGroups);

			Assert.True(permissionGroups.Any(pg => pg.Group == "android.permission-group.CAMERA"
											 && pg.Permissions.Any(p => p.Permission == "android.permission.CAMERA")));
		}


		[Fact]
		public void Test_List_Permission_Groups()
		{
			var permissionGroups = Cake.PmListPermissionGroups(settings: GetAdbToolSettings());

			Assert.NotEmpty(permissionGroups);
			Assert.True(permissionGroups.Contains("android.permission-group.CAMERA"));
		}

		[Fact]
		public void Test_List_Features()
		{
			var features = Cake.PmListFeatures(GetAdbToolSettings());

			Assert.NotEmpty(features);
			Assert.True(features.Contains("android.hardware.location"));
		}

		[Fact]
		public void Test_List_Libraries()
		{
			var libs = Cake.PmListLibraries(GetAdbToolSettings());

			Assert.NotEmpty(libs);
			Assert.True(libs.Contains("com.android.location.provider"));
		}

		[Fact]
		public void Test_List_Packages()
		{
			var packages = Cake.PmListPackages(settings: GetAdbToolSettings());

			Assert.NotEmpty(packages);
			Assert.True(packages.Any(p => p.PackageName == "com.android.providers.downloads"));
		}

		[Fact]
		public void Test_Path_to_Package()
		{
			var path = Cake.PmPathToPackage("com.android.providers.downloads", settings: GetAdbToolSettings());

			Assert.NotNull(path);
			Assert.True(path.FullPath == "/system/priv-app/DownloadProvider/DownloadProvider.apk");
		}
	}
}
