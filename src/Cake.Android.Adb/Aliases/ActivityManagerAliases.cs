using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.AndroidAdb
{
	/// <summary>
	/// Cake build aliases for Android ADB Activity Manager commands
	/// </summary>
	[CakeAliasCategory ("Android")]
	public static class ActivityManagerAliases
	{
		static AdbTool GetAdbTool(ICakeContext context)
		{
			return new AdbTool(context, context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
		}

		/// <summary>
		/// Starts an Activity on the target.
		/// </summary>
		/// <returns><c>true</c>, if an activity was started, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="adbIntentArguments">Intent to start the activity with, as per the Intent Spec: https://developer.android.com/studio/command-line/adb.html#IntentSpec.</param>
		/// <param name="options">Options.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AmStartActivity(this ICakeContext context, string adbIntentArguments, AmStartOptions options = null, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.StartActivity(adbIntentArguments, options, settings);
		}

		/// <summary>
		/// Starts a Service on the target.
		/// </summary>
		/// <returns><c>true</c>, if service was started, <c>false</c> otherwise.</returns>
		/// <param name="context">Context.</param>
		/// <param name="adbIntentArguments">Intent to start the service with, as per the Intent Spec: https://developer.android.com/studio/command-line/adb.html#IntentSpec.</param>
		/// <param name="runAsUser">Run as user.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static bool AmStartService(this ICakeContext context, string adbIntentArguments, string runAsUser = null, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.StartService(adbIntentArguments, runAsUser, settings);
		}

		/// <summary>
		/// Force stops an application on the target.
		/// </summary>
		/// <returns>The force stop.</returns>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AmForceStop(this ICakeContext context, string packageName, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.ForceStop(packageName, settings);
		}

		/// <summary>
		/// Kills a service on the target.
		/// </summary>
		/// <returns>The kill.</returns>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="forUser">For user.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AmKill(this ICakeContext context, string packageName, string forUser = null, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.Kill(packageName, forUser, settings);
		}

		/// <summary>
		/// Kills all services on the target.
		/// </summary>
		/// <returns>The kill all.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static void AmKillAll(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			t.KillAll(settings);
		}

		/// <summary>
		/// Broadcasts an Intent on the target.
		/// </summary>
		/// <returns>The broadcast.</returns>
		/// <param name="context">Context.</param>
		/// <param name="intent">Intent spec to broadcast, as per the Intent Spec: https://developer.android.com/studio/command-line/adb.html#IntentSpec.</param>
		/// <param name="toUser">To user.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static int AmBroadcast(this ICakeContext context, string intent, string toUser = null, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Broadcast(intent, toUser, settings);
		}

		/// <summary>
		/// Ams the instrument.
		/// </summary>
		/// <returns>The instrument.</returns>
		/// <param name="context">Context.</param>
		/// <param name="component">Component.</param>
		/// <param name="options">Options.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmInstrument(this ICakeContext context, string component, AmInstrumentOptions options = null, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Instrument(component, options, settings);
		}

		/// <summary>
		/// Starts profiling on the target.
		/// </summary>
		/// <returns>The start profiling.</returns>
		/// <param name="context">Context.</param>
		/// <param name="process">Process to profile.</param>
		/// <param name="outputFile">Output file to save profile data to.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmStartProfiling(this ICakeContext context, string process, FilePath outputFile, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.StartProfiling(process, outputFile, settings);
		}

		/// <summary>
		/// Stops profiling on the target.
		/// </summary>
		/// <returns>The stop profiling.</returns>
		/// <param name="context">Context.</param>
		/// <param name="process">Process to stop profiling.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmStopProfiling(this ICakeContext context, string process, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.StopProfiling(process, settings);
		}

		/// <summary>
		/// Dumps the heap from a process on the target.
		/// </summary>
		/// <returns>The dump heap.</returns>
		/// <param name="context">Context.</param>
		/// <param name="process">Process to dump heap for.</param>
		/// <param name="outputFile">Output file to save heap dump to.</param>
		/// <param name="forUser">Restrict heap dump to a user.</param>
		/// <param name="dumpNativeHeap">If set to <c>true</c> dump native heap instead.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmDumpHeap(this ICakeContext context, string process, FilePath outputFile, string forUser = null, bool dumpNativeHeap = false, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.DumpHeap(process, outputFile, forUser, dumpNativeHeap, settings);
		}

		/// <summary>
		/// Set the debug app on the target.
		/// </summary>
		/// <returns>The set debug app.</returns>
		/// <param name="context">Context.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="wait">If set to <c>true</c> wait until operation is complete.</param>
		/// <param name="persistent">If set to <c>true</c> persist this setting..</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmSetDebugApp(this ICakeContext context, string packageName, bool wait = false, bool persistent = false, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.SetDebugApp(packageName, wait, persistent, settings);
		}

		/// <summary>
		/// Clears the set debug app on the target.
		/// </summary>
		/// <returns>The clear debug app.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmClearDebugApp(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.ClearDebugApp(settings);
		}

		/// <summary>
		/// Starts monitoring for crashes or ANRs on the target.
		/// </summary>
		/// <returns>The monitor.</returns>
		/// <param name="context">Context.</param>
		/// <param name="gdbPort">Gdb port to start gdbserv on when crash/ANR occurs.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmMonitor(this ICakeContext context, int? gdbPort = null, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.Monitor(gdbPort, settings);
		}

		/// <summary>
		/// Turn screen compatibility mode on/off for the given package on the target.
		/// </summary>
		/// <returns>The screen compat.</returns>
		/// <param name="context">Context.</param>
		/// <param name="compatOn">If set to <c>true</c> compatibility will be turned on.</param>
		/// <param name="packageName">Package name.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmScreenCompat(this ICakeContext context, bool compatOn, string packageName, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.ScreenCompat(compatOn, packageName, settings);
		}

		/// <summary>
		/// Override the target's display size. This command is helpful for testing your app across different screen sizes by mimicking a small screen resolution using a device with a large screen, and vice versa.
		/// </summary>
		/// <returns>The display size.</returns>
		/// <param name="context">Context.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmDisplaySize(this ICakeContext context, int width, int height, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.DisplaySize(width, height, settings);
		}

		/// <summary>
		/// Resets the target's display size.
		/// </summary>
		/// <returns>The reset display size.</returns>
		/// <param name="context">Context.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmResetDisplaySize(this ICakeContext context, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.ResetDisplaySize(settings);
		}

		/// <summary>
		/// Override the target's display density. This command is helpful for testing your app across different screen densities on high-density screen environment using a low density screen, and vice versa.
		/// </summary>
		/// <returns>The display density.</returns>
		/// <param name="context">Context.</param>
		/// <param name="dpi">Density in dpi.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static List<string> AmDisplayDensity(this ICakeContext context, int dpi, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.DisplayDensity(dpi, settings);
		}

		/// <summary>
		/// Convert the given intent specification to a URI.
		/// </summary>
		/// <returns>The intent URI.</returns>
		/// <param name="context">Context.</param>
		/// <param name="intent">Intent spec to broadcast, as per the Intent Spec: https://developer.android.com/studio/command-line/adb.html#IntentSpec.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static string AmIntentToURI(this ICakeContext context, string intent, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.IntentToURI(intent, settings);
		}

		/// <summary>
		/// Convert the given intent specification to an Intent URI.
		/// </summary>
		/// <returns>The intent URI.</returns>
		/// <param name="context">Context.</param>
		/// <param name="intent">Intent spec to broadcast, as per the Intent Spec: https://developer.android.com/studio/command-line/adb.html#IntentSpec.</param>
		/// <param name="settings">Settings.</param>
		[CakeMethodAlias]
		public static string AmIntentToIntentURI(this ICakeContext context, string intent, AdbToolSettings settings = null)
		{
			var t = GetAdbTool(context);
			return t.IntentToIntentURI(intent, settings);
		}
	}
}
