using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AndroidAdb
{
	/// <summary>
	/// Enabled/Disabled state of packages.
	/// </summary>
	public enum PackageListState
	{
		/// <summary>
		/// All - Enabled and Disabled.
		/// </summary>
		All,
		/// <summary>
		/// Only enabled.
		/// </summary>
		OnlyEnabled,
		/// <summary>
		/// Only Disabled.
		/// </summary>
		OnlyDisabled
	}

	/// <summary>
	/// Source type of packages
	/// </summary>
	public enum PackageSourceType
	{
		/// <summary>
		/// All - System and Third Party.
		/// </summary>
		All,
		/// <summary>
		/// Only System.
		/// </summary>
		OnlySystem,
		/// <summary>
		/// Only Third Party.
		/// </summary>
		OnlyThirdParty
	}

	/// <summary>
	/// Install Location of packages.
	/// </summary>
	public enum AdbInstallLocation
	{
		/// <summary>
		/// Auto - Let the system automatically decide.
		/// </summary>
		Auto = 0,
		/// <summary>
		/// Internal - System Memory.
		/// </summary>
		Internal = 1,
		/// <summary>
		/// External - Mass Storage Device.
		/// </summary>
		External = 2
	}

	partial class AdbTool
	{
		public List<AdbPackageListInfo> ListPackages(bool includeUninstalled = false, PackageListState showState = PackageListState.All, PackageSourceType showSource = PackageSourceType.All, AdbToolSettings settings = null)
		{
			// list packages [options] filter
			if (settings == null)
				settings = new AdbToolSettings();

			// start [options] intent
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("list");
			builder.Append("packages");
			builder.Append("-f");
			builder.Append("-i");

			if (showState == PackageListState.OnlyDisabled)
				builder.Append("-d");
			else if (showState == PackageListState.OnlyEnabled)
				builder.Append("-e");
			
			if (showSource == PackageSourceType.OnlySystem)
				builder.Append("-s");
			else if (showSource == PackageSourceType.OnlyThirdParty)
				builder.Append("-3");

			if (includeUninstalled)
				builder.Append("-u");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			var results = new List<AdbPackageListInfo>();

			const string rxPackageListInfo = "^package:(?<path>.*?)=(?<package>.*?)\\s+installer=(?<installer>.*?)$";
			foreach (var line in output)
			{
				var m = Regex.Match(line, rxPackageListInfo, RegexOptions.Singleline);

				var installPath = m?.Groups?["path"]?.Value;
				var packageName = m?.Groups?["package"]?.Value;
				var installer = m?.Groups?["installer"]?.Value;

				if (!string.IsNullOrEmpty(installPath) && !string.IsNullOrEmpty(packageName))
					results.Add(new AdbPackageListInfo {
						InstallPath = new FilePath (installPath),
						PackageName = packageName,
						Installer = installer,
					});

			}
			return results;
		}


		public List<string> ListPermissionGroups(AdbToolSettings settings = null)
		{
			// list packages [options] filter
			if (settings == null)
				settings = new AdbToolSettings();

			// start [options] intent
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("list");
			builder.Append("permission-groups");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			var results = new List<string>();

			const string rxPackageListInfo = "^permission group:(?<group>.*?)$";
			foreach (var line in output)
			{
				var m = Regex.Match(line, rxPackageListInfo, RegexOptions.Singleline);

				var pg = m?.Groups?["group"]?.Value;

				if (!string.IsNullOrEmpty(pg))
					results.Add(pg);
			}
			return results;
		}

		public List<AdbPermissionGroupInfo> ListPermissions(bool onlyDangerous = false, bool onlyUserVisible = false, AdbToolSettings settings = null)
		{
			// list packages [options] filter
			if (settings == null)
				settings = new AdbToolSettings();

			// start [options] intent
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("list");
			builder.Append("permissions");
			builder.Append("-g");
			builder.Append("-f");

			if (onlyDangerous)
				builder.Append("-d");
			if (onlyUserVisible)
				builder.Append("-u");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			var results = new List<AdbPermissionGroupInfo>();

			AdbPermissionGroupInfo currentGroup = null;
			AdbPermissionInfo currentPerm = null;

			foreach (var line in output)
			{
				if (string.IsNullOrWhiteSpace(line) || line.StartsWith("All Permissions:", StringComparison.OrdinalIgnoreCase))
					continue;

				if (line.StartsWith("+ group:"))
				{
					if (currentPerm != null)
					{
						currentGroup.Permissions.Add(currentPerm);
						currentPerm = null;
					}

					if (currentGroup != null)
						results.Add(currentGroup);
					
					currentGroup = new AdbPermissionGroupInfo();
					currentGroup.Group = line.Substring(8);
				}
				else if (line.StartsWith("  package:"))
				{
					currentGroup.PackageName = line.Substring(10);
				}
				else if (line.StartsWith("  label:"))
				{
					currentGroup.Label = line.Substring(8);
				}
				else if (line.StartsWith("  description:"))
				{
					currentGroup.Label = line.Substring(14);
				}
				else if (line.StartsWith("  + permission:"))
				{
					if (currentPerm != null && currentGroup != null)
						currentGroup.Permissions.Add(currentPerm);
					
					currentPerm = new AdbPermissionInfo(); 
					currentPerm.Permission = line.Substring(15);
				}
				else if (line.StartsWith("    package:"))
				{
					currentPerm.PackageName = line.Substring(12);
				}
				else if (line.StartsWith("    label:"))
				{
					currentPerm.Label = line.Substring(10);
				}
				else if (line.StartsWith("    description:"))
				{
					currentPerm.Description = line.Substring(16);
				}
				else if (line.StartsWith("    protectionLevel:"))
				{
					var plraw = line.Substring(20);
					currentPerm.ProtectionLevels.AddRange(plraw.Split('|'));
				}
			}

			if (currentPerm != null && currentGroup != null)
				currentGroup.Permissions.Add(currentPerm);

			if (currentGroup != null)
				results.Add(currentGroup);
			
			return results;
		}


		public List<string> ListFeatures(AdbToolSettings settings = null)
		{
			// list packages [options] filter
			if (settings == null)
				settings = new AdbToolSettings();

			// start [options] intent
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("list");
			builder.Append("features");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			var results = new List<string>();

			const string rxPackageListInfo = "^feature:(?<feature>.*?)$";
			foreach (var line in output)
			{
				var m = Regex.Match(line, rxPackageListInfo, RegexOptions.Singleline);

				var ft = m?.Groups?["feature"]?.Value;

				if (!string.IsNullOrEmpty(ft))
					results.Add(ft);
			}
			return results;
		}

		public List<string> ListLibraries(AdbToolSettings settings = null)
		{
			// list packages [options] filter
			if (settings == null)
				settings = new AdbToolSettings();

			// start [options] intent
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("list");
			builder.Append("libraries");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			var results = new List<string>();

			const string rxPackageListInfo = "^library:(?<lib>.*?)$";
			foreach (var line in output)
			{
				var m = Regex.Match(line, rxPackageListInfo, RegexOptions.Singleline);

				var lib = m?.Groups?["lib"]?.Value;

				if (!string.IsNullOrEmpty(lib))
					results.Add(lib);
			}
			return results;
		}


		public FilePath PathToPackage(string packageName, AdbToolSettings settings = null)
		{
			// list packages [options] filter
			if (settings == null)
				settings = new AdbToolSettings();

			// start [options] intent
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("path");
			builder.Append(packageName);

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			const string rxPackageListInfo = "^package:(?<path>.*?)$";
		    var outputList = output.Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
			foreach (var line in outputList)
			{
				var m = Regex.Match(line, rxPackageListInfo, RegexOptions.Singleline);

				var path = m?.Groups?["path"]?.Value;

			    if (!string.IsNullOrWhiteSpace(path))
			    {
				    var fp = new FilePath(path);

				    return fp;
			    }
			}

			return null;
		}


		public void Install(FilePath pathOnDevice, 
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
			// install[options] path
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("install");

			if (forwardLock)
				builder.Append("-l");
			if (reinstall)
				builder.Append("-r");
			if (allowTestApks)
				builder.Append("-t");
			if (installOnSharedStorage)
				builder.Append("-s");
			if (installOnInternalSystemMemory)
				builder.Append("-f");
			if (allowVersionDowngrade)
				builder.Append("-d");
			if (grantAllManifestPermissions)
				builder.Append("-g");

			builder.AppendQuoted(pathOnDevice.FullPath);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}


		//public void Uninstall(string packageName, bool keepDataAndCache = false, AdbToolSettings settings = null)
		//{
		//	// uninstall [options] package
		//	if (settings == null)
		//		settings = new AdbToolSettings();

		//	var builder = new ProcessArgumentBuilder();

		//	AddSerial(settings.Serial, builder);

		//	builder.Append("shell");
		//	builder.Append("pm");

		//	builder.Append("uninstall");

		//	if (keepDataAndCache)
		//		builder.Append("-k");

		//	builder.Append(packageName);

		//	var output = new List<string>();
		//	RunAdb(settings, builder, out output);
		//}

		public void Clear(string packageName, AdbToolSettings settings = null)
		{
			// clear package
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("clear");
			builder.Append(packageName);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void Enable(string packageOrComponent, AdbToolSettings settings = null)
		{
			// clear package
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("enable");
			builder.Append(packageOrComponent);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void Disable(string packageOrComponent, AdbToolSettings settings = null)
		{
			// clear package
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("disable");
			builder.Append(packageOrComponent);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void DisableUser(string packageOrComponent, string forUser = null, AdbToolSettings settings = null)
		{
			// disable-user [options] package_or_component
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("disable");

			if (!string.IsNullOrEmpty(forUser))
			{
				builder.Append("--user");
				builder.Append(forUser);
			}

			builder.Append(packageOrComponent);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void Grant(string packageName, string permission, AdbToolSettings settings = null)
		{
			// grant package_name permission
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("grant");
			builder.Append(packageName);
			builder.Append(permission);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void Revoke(string packageName, string permission, AdbToolSettings settings = null)
		{
			// revoke package_name permission
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("revoke");
			builder.Append(packageName);
			builder.Append(permission);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void SetInstallLocation(AdbInstallLocation location, AdbToolSettings settings = null)
		{
			// set-install-location location
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("set-install-location");
			builder.Append(((int)location).ToString ());

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public AdbInstallLocation GetInstallLocation(AdbToolSettings settings = null)
		{
			// set-install-location location
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("get-install-location");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			var o = string.Join(Environment.NewLine, output);

			if (o.Contains("[internal]"))
				return AdbInstallLocation.Internal;
			if (o.Contains("[external]"))
				return AdbInstallLocation.External;

			return AdbInstallLocation.Auto;
		}


		public void SetPermissionEnforced(string permission, bool enforced, AdbToolSettings settings = null)
		{
			// set-permission-enforced permission [true|false]
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("set-permission-enforced");
			builder.Append(permission);
			builder.Append(enforced ? "true" : "false");

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void TrimCaches(string desiredFreeSpace, AdbToolSettings settings = null)
		{
			// trim-caches desired_free_space
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("trim-caches");
			builder.Append(desiredFreeSpace);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void CreateUser(string userName, AdbToolSettings settings = null)
		{
			// create-user user_name
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("create-user");
			builder.Append(userName);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void RemoveUser(string userId, AdbToolSettings settings = null)
		{
			// remove-user user_id
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("remove-user");
			builder.Append(userId);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public int GetMaxUsers(AdbToolSettings settings = null)
		{
			// get-max-users
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("pm");

			builder.Append("get-max-users");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			var o = output.FirstOrDefault() ?? string.Empty;

			if (o.StartsWith("Maximum supported users:", StringComparison.OrdinalIgnoreCase))
			{
				int result = -1;
				var num = o.Substring(24).Trim();
				if (int.TryParse(num, out result))
					return result;
			}

			return -1;
		}
	}
}
