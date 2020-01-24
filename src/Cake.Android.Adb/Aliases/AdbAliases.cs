using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.AndroidAdb
{
	/// <summary>
	/// Cake build aliases for Android ADB commands
	/// </summary>
	[CakeAliasCategory("Android")]
	public static class AdbAliases
	{
		static AdbTool GetAdbTool(ICakeContext context)
		{
			return new AdbTool(context, context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
		}

		/// <summary>
		/// Gets a list of all attached emulator/device instances.
		/// </summary>
		/// <returns>List of Devices.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<AdbDeviceInfo> AdbDevices(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.GetDevices(settings);
		}

		/// <summary>
		/// Gets the adb version number.
		/// </summary>
		/// <returns>Version.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static string AdbVersion(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Version(settings);
		}

		/// <summary>
		/// Terminates the adb server process.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AdbKillServer(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.KillServer(settings);
		}

		/// <summary>
		/// Checks whether the adb server process is running and starts it, if not.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AdbStartServer(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.StartServer(settings);
		}

		/// <summary>
		/// Connects to a device or emulator by IP address.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="deviceIp">Target IP address.</param>
		/// <param name="port">Target port.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AdbConnect(this ICakeContext context, string deviceIp, int port = 5555, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.Connect(deviceIp, port, settings);
		}

		/// <summary>
		/// Disconnects from a device or emulator.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="deviceIp">Target IP address.</param>
		/// <param name="port">Port.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AdbDisconnect(this ICakeContext context, string deviceIp = null, int port = 5555, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.Disconnect(deviceIp, port, settings);
		}

		/// <summary>
		/// Installs an Android .APK file.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="apkFile">Local .APK file to install.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AdbInstall(this ICakeContext context, FilePath apkFile, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.Install(apkFile, settings);
		}

		/// <summary>
		/// Uninstalls an application from the target.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name to uninstall.</param>
		/// <param name="keepDataAndCacheDirs">If set to <c>true</c> keep data and cache directories.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AdbUninstall(this ICakeContext context, string packageName, bool keepDataAndCacheDirs = false, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.Uninstall(packageName, keepDataAndCacheDirs, settings);
		}

		/// <summary>
		/// Pulls a file from the target to a local destination.
		/// </summary>
		/// <returns><c>true</c>, if pull succeeded, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="remoteFileSource">Remote file to pull.</param>
		/// <param name="localFileDestination">Local file destination.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AdbPull(this ICakeContext context, FilePath remoteFileSource, FilePath localFileDestination, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Pull(remoteFileSource, localFileDestination, settings);
		}

		/// <summary>
		/// Pulls a directory from the target to a local destination.
		/// </summary>
		/// <returns><c>true</c>, if pull succeeded, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="remoteDirectorySource">Remote directory to pull.</param>
		/// <param name="localDirectoryDestination">Local destination.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AdbPull(this ICakeContext context, DirectoryPath remoteDirectorySource, DirectoryPath localDirectoryDestination, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Pull(remoteDirectorySource, localDirectoryDestination, settings);
		}

		/// <summary>
		/// Pulls a file from the target to a local destination.
		/// </summary>
		/// <returns><c>true</c>, if pull succeeded, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="remoteFileSource">Remote file to pull.</param>
		/// <param name="localDirectoryDestination">Local destination.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AdbPull(this ICakeContext context, FilePath remoteFileSource, DirectoryPath localDirectoryDestination, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Pull(remoteFileSource, localDirectoryDestination, settings);
		}

		/// <summary>
		/// Pushes a local file to the remote destination on the target.
		/// </summary>
		/// <returns><c>true</c>, if push succeeded, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="localFileSource">Local file source.</param>
		/// <param name="remoteFileDestination">Remote file destination.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AdbPush(this ICakeContext context, FilePath localFileSource, FilePath remoteFileDestination, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Push(localFileSource, remoteFileDestination, settings);
		}

		/// <summary>
		/// Pushes a local file to the remote destination on the target.
		/// </summary>
		/// <returns><c>true</c>, if push succeeded, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="localFileSource">Local file source.</param>
		/// <param name="remoteDirectoryDestination">Remote destination.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AdbPush(this ICakeContext context, FilePath localFileSource, DirectoryPath remoteDirectoryDestination, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Push(localFileSource, remoteDirectoryDestination, settings);
		}

		/// <summary>
		/// Pushes a local directory to the remote destination on the target.
		/// </summary>
		/// <returns><c>true</c>, if push succeeded, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="localDirectorySource">Local file source.</param>
		/// <param name="remoteDirectoryDestination">Remote file destination.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AdbPush(this ICakeContext context, DirectoryPath localDirectorySource, DirectoryPath remoteDirectoryDestination, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Push(localDirectorySource, remoteDirectoryDestination, settings);
		}

		/// <summary>
		/// Dumps out a bug report.
		/// </summary>
		/// <returns>The bug report.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AdbBugReport(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.BugReport(settings);
		}

		/// <summary>
		/// Dumps out Logcat output.
		/// </summary>
		/// <returns>The logcat.</returns>
		/// <param name="context">Context.</param>
		/// <param name="options">Options.</param>
		/// <param name="filter">Filter.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AdbLogcat(this ICakeContext context, AdbLogcatOptions options = null, string filter = null, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Logcat(options, filter, settings);
		}

		/// <summary>
		/// Gets the target's serial.
		/// </summary>
		/// <returns>The serial number.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static string AdbGetSerialNumber(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.GetSerialNumber(settings);
		}

		/// <summary>
		/// Gets the target's state.
		/// </summary>
		/// <returns>The state.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static string AdbGetState(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.GetState(settings);
		}

		/// <summary>
		/// Executes a shell command on the target.
		/// </summary>
		/// <returns>The shell command output.</returns>
		/// <param name="context">Context.</param>
		/// <param name="shellCommand">Shell command to execute.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AdbShell(this ICakeContext context, string shellCommand, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Shell(shellCommand, settings);
		}

		/// <summary>
		/// Captures a screenshot from the target.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="saveToLocalFile">Save to local file.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AdbScreenCapture(this ICakeContext context, FilePath saveToLocalFile, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.ScreenCapture(saveToLocalFile, settings);
		}

		/// <summary>
		/// Records the screen of the target.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="saveToLocalFile">Save to local file.</param>
		/// <param name="recordingCancelToken">Recording cancel token.  If specified, the cancel token must be cancelled to stop the recording.</param>
		/// <param name="timeLimit">Time limit.  If specified, recording will stop after the specified time.</param>
		/// <param name="bitrateMbps">Bitrate quality in megabits per second.  Default is 4mbps.</param>
		/// <param name="width">Recording width.</param>
		/// <param name="height">Recording height.</param>
		/// <param name="rotate">If set to <c>true</c> rotate the recording 90 degrees.</param>
		/// <param name="logVerbose">If set to <c>true</c> show verbose log output.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AdbScreenRecord(this ICakeContext context, FilePath saveToLocalFile, System.Threading.CancellationToken? recordingCancelToken = null, TimeSpan? timeLimit = null, int? bitrateMbps = null, int? width = null, int? height = null, bool rotate = false, bool logVerbose = false, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.ScreenRecord(saveToLocalFile, recordingCancelToken, timeLimit, bitrateMbps, width, height, rotate, logVerbose, settings);
		}

		/// <summary>
		/// Connects to an emulator and queries its avd name
		/// </summary>
		/// <returns>AVD name of the emulator.</returns>
		/// <param name="context">Context.</param>
		/// <param name="emulatorSerial">Emulator serial to get AVD name of.  Must be in the format 'emulator-5554'.</param>
		[CakeMethodAlias]
		public static string AdbGetAvdName(this ICakeContext context, string emulatorSerial)
		{
			return AdbNetworkClient.GetAvdName(emulatorSerial);
		}

		/// <summary>
		/// Waits for an emulator to boot (dev.bootcomplete=1)
		/// </summary>
		/// <returns><c>true</c>, if emulator booted, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="timeout">Timeout.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AdbWaitForEmulatorToBoot(this ICakeContext context, TimeSpan timeout, AdbToolSettings settings = null)
		{
			var booted = false;
			for (int i = 0; i < timeout.TotalSeconds; i++)
			{
				if (AdbShell(context, "getprop dev.bootcomplete", settings).Any(l => l.Contains("1")))
				{
					booted = true;
					break;
				}
				else
				{
					System.Threading.Thread.Sleep(1000);
				}
			}
			return booted;
		}

		/// <summary>
		/// Waits for a given transport and state
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="transport">Transport.</param>
		/// <param name="state">State.</param>
		[CakeMethodAlias]
		public static void AdbWaitFor(this ICakeContext context, AdbTransport transport = AdbTransport.Any, AdbState state = AdbState.Device)
		{
			var t = GetAdbTool(context);
			t.WaitFor(transport, state);
		}
	}
}
