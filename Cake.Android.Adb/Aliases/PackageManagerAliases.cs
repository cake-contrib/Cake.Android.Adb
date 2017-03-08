using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.AndroidAdb
{
	/// <summary>
	/// Cake build aliases for Android ADB Package Manager commands
	/// </summary>
	[CakeAliasCategory ("Android")]
	public static class PackageManagerAliases
	{
		
		/// <summary>
		/// Gets a list of packages from the target.
		/// </summary>
		/// <returns>The list of packages.</returns>
		/// <param name="context">Context.</param>
		/// <param name="includeUninstalled">If set to <c>true</c> include uninstalled packages.</param>
		/// <param name="showState">Show All by default, or choose to show only enabled or disabled packages.</param>
		/// <param name="showSource">Show All by default, or choose to show only System or 3rd party packages.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<AdbPackageListInfo> PmListPackages(this ICakeContext context, bool includeUninstalled, PackageListState showState, PackageSourceType showSource, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			return t.ListPackages(includeUninstalled, showState, showSource, settings);
		}

		/// <summary>
		/// Gets a list of Permission Groups on the target.
		/// </summary>
		/// <returns>The list of permission groups.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> PmListPermissionGroups(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			return t.ListPermissionGroups(settings);
		}

		/// <summary>
		/// Gets a list of Permissions, grouped by Permission Group on the target
		/// </summary>
		/// <returns>The list of Permissions grouped by Permission Group.</returns>
		/// <param name="context">Context.</param>
		/// <param name="onlyDangerous">If set to <c>true</c> return only permissions marked dangerous.</param>
		/// <param name="onlyUserVisible">If set to <c>true</c> return only permissions visible to the user.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<AdbPermissionGroupInfo> PmListPermissions(this ICakeContext context, bool onlyDangerous = false, bool onlyUserVisible = false, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			return t.ListPermissions(onlyDangerous, onlyUserVisible, settings);
		}

		/// <summary>
		/// Gets a list of features implemented on the target.
		/// </summary>
		/// <returns>The list of features.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> PmListFeatures(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			return t.ListFeatures(settings);
		}

		/// <summary>
		/// Gets a list of libraries that exist on the target.
		/// </summary>
		/// <returns>The list of libraries.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> PmListLibraries(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			return t.ListLibraries(settings);
		}

		/// <summary>
		/// Gets the path for a given package name.
		/// </summary>
		/// <returns>The path to package.</returns>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static FilePath PmPathToPackage(this ICakeContext context, string packageName, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			return t.PathToPackage(packageName, settings);
		}

		/// <summary>
		/// Installs an APK file from the given path on the target.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="pathOnDevice">Path of the APK to install on the target.</param>
		/// <param name="forwardLock">If set to <c>true</c> install the package with a forward lock.</param>
		/// <param name="reinstall">If set to <c>true</c> reinstall the package, keeping its data.</param>
		/// <param name="allowTestApks">If set to <c>true</c> allow test APKs to be installed.</param>
		/// <param name="installerPackageName">Installer package name.</param>
		/// <param name="installOnSharedStorage">If set to <c>true</c> install on shared storage.</param>
		/// <param name="installOnInternalSystemMemory">If set to <c>true</c> install on internal system memory.</param>
		/// <param name="allowVersionDowngrade">If set to <c>true</c> allow version downgrade.</param>
		/// <param name="grantAllManifestPermissions">If set to <c>true</c> grant all manifest permissions.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmInstall(this ICakeContext context, 
									FilePath pathOnDevice,
									bool forwardLock = false,
									bool reinstall = false,
									bool allowTestApks = false,
									string installerPackageName = null,
									bool installOnSharedStorage = false,
									bool installOnInternalSystemMemory = false,
									bool allowVersionDowngrade = false,
									bool grantAllManifestPermissions = false,
									AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.Install(pathOnDevice, forwardLock, reinstall, allowTestApks, installerPackageName, installOnSharedStorage, installOnInternalSystemMemory, allowVersionDowngrade, grantAllManifestPermissions, settings);
		}

		/// <summary>
		/// Uninstalls a package from the target
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="keepDataAndCache">If set to <c>true</c> keep data and cache.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmUninstall(this ICakeContext context, string packageName, bool keepDataAndCache = false, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.Uninstall(packageName, keepDataAndCache, settings);
		}

		/// <summary>
		/// Clears all data associated with the package.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmClear(this ICakeContext context, string packageName, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.Clear(packageName, settings);
		}

		/// <summary>
		/// Enables the given package or component
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="packageOrComponent">Package or component (written as "package/class").</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmEnable(this ICakeContext context, string packageOrComponent, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.Enable(packageOrComponent, settings);
		}

		/// <summary>
		/// Disables the given package or component
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="packageOrComponent">Package or component (written as "package/class").</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmDisable(this ICakeContext context, string packageOrComponent, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.Disable(packageOrComponent, settings);
		}

		/// <summary>
		/// Disables a user for the given package or component.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="packageOrComponent">Package or component (written as "package/class").</param>
		/// <param name="forUser">For user.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmDisableUser(this ICakeContext context, string packageOrComponent, string forUser = null, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.DisableUser(packageOrComponent, forUser, settings);
		}

		/// <summary>
		/// Grants a permission to a package.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="permission">Permission.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmGrant(this ICakeContext context, string packageName, string permission, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.Grant(packageName, permission, settings);
		}

		/// <summary>
		/// Revokes a permission from a package.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="permission">Permission.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmRevoke(this ICakeContext context, string packageName, string permission, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.Revoke(packageName, permission, settings);
		}

		/// <summary>
		/// Sets the default install location for the target.  Note: This is only intended for debugging; using this can cause applications to break and other undesireable behavior.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="location">Location.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmSetInstallLocation(this ICakeContext context, AdbInstallLocation location, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.SetInstallLocation(location, settings);
		}

		/// <summary>
		/// Gets the current default install location for the target.
		/// </summary>
		/// <returns>The get install location.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static AdbInstallLocation PmGetInstallLocation(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			return t.GetInstallLocation(settings);
		}

		/// <summary>
		/// Sets whether or not a permission is enforced on the target.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="permission">Permission.</param>
		/// <param name="enforced">If set to <c>true</c> enforced.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmSetPermissionEnforced(this ICakeContext context, string permission, bool enforced, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.SetPermissionEnforced(permission, enforced, settings);
		}

		/// <summary>
		/// Tries to free up space on the target by deleting caches.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="desiredFreeSpace">Desired free space to trim.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmTrimCaches(this ICakeContext context, string desiredFreeSpace, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.TrimCaches(desiredFreeSpace, settings);
		}

		/// <summary>
		/// Creates a new user on the target.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="userName">User name.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmCreateUser(this ICakeContext context, string userName, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.CreateUser(userName, settings);
		}

		/// <summary>
		/// Removes a user from a target.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void PmRemoveUser(this ICakeContext context, string userId, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			t.RemoveUser(userId, settings);
		}

		/// <summary>
		/// Gets the max # of users the target supports.
		/// </summary>
		/// <returns>The get max users.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static int PmGetMaxUsers(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = AdbRunnerFactory.GetAdbTool(context);
			return t.GetMaxUsers(settings);
		}
	}
}
