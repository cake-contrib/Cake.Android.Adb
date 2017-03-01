using NUnit.Framework;
using System;
using Cake.AndroidAdb.Fakes;
using System.Linq;

namespace Cake.AndroidAdb.Tests
{
	[TestFixture]
	public class AdbPackageManagerTests : TestFixtureBase
	{

		[Test]
		public void Test_List_Permissions ()
		{
			var permissionGroups = Cake.PmListPermissions(settings: GetAdbToolSettings());

			Assert.IsNotEmpty(permissionGroups);

			Assert.True(permissionGroups.Any(pg => pg.Group == "android.permission-group.CAMERA" 
			                                 && pg.Permissions.Any(p => p.Permission == "android.permission.CAMERA")));
		}


		[Test]
		public void Test_List_Permission_Groups()
		{
			var permissionGroups = Cake.PmListPermissionGroups(settings:GetAdbToolSettings ());

			Assert.IsNotEmpty(permissionGroups);
			Assert.True(permissionGroups.Contains("android.permission-group.CAMERA"));
		}

		[Test]
		public void Test_List_Features()
		{
			var features = Cake.PmListFeatures(GetAdbToolSettings());

			Assert.IsNotEmpty(features);
			Assert.True(features.Contains("android.hardware.location"));
		}

		[Test]
		public void Test_List_Libraries()
		{
			var libs = Cake.PmListLibraries(GetAdbToolSettings());

			Assert.IsNotEmpty(libs);
			Assert.True(libs.Contains("com.android.location.provider"));
		}

		[Test]
		public void Test_List_Packages()
		{
			var packages = Cake.PmListPackages(settings: GetAdbToolSettings());

			Assert.IsNotEmpty(packages);
			Assert.True(packages.Any (p => p.PackageName == "com.android.providers.downloads"));
		}

        [Test]
        public void Test_Path_to_Package()
        {
            var path = Cake.PmPathToPackage("com.android.providers.downloads", settings: GetAdbToolSettings());

            Assert.IsNotNull(path);
            Assert.True(path.FullPath == "/system/priv-app/DownloadProvider/DownloadProvider.apk");
        }
    }
}
