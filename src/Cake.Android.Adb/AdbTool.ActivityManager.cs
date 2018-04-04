using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.AndroidAdb
{
	partial class AdbTool
	{
		public bool StartActivity(string adbIntentArguments, AmStartOptions options = null, AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			if (options == null)
				options = new AmStartOptions();

			// start [options] intent
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("start");

			if (options.EnableDebugging)
				builder.Append("-D");
			if (options.WaitForLaunch)
				builder.Append("-W");
			if (options.ProfileToFile != null)
			{
				if (options.ProfileUntilIdle)
					builder.Append("-P");
				else
					builder.Append("--start");
				builder.AppendQuoted(options.ProfileToFile.MakeAbsolute(environment).FullPath);
			}
			if (options.RepeatLaunch.HasValue && options.RepeatLaunch.Value > 0)
			{
				builder.Append("-R");
				builder.Append(options.RepeatLaunch.Value.ToString());
			}
			if (options.ForceStopTarget)
				builder.Append("-S");
			if (options.EnableOpenGLTrace)
				builder.Append("--opengl-trace");
			if (!string.IsNullOrEmpty(options.RunAsUserId))
			{
				builder.Append("--user");
				builder.Append(options.RunAsUserId);
			}

			builder.Append(adbIntentArguments);

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return output.Any(l => l.StartsWith("Starting:", StringComparison.OrdinalIgnoreCase));
		}

		public bool StartService(string adbIntentArguments, string runAsUser = null, AdbToolSettings settings = null)
		{
			// startservice [options] intent
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("startservice");

			if (!string.IsNullOrEmpty(runAsUser))
			{
				builder.Append("--user");
				builder.Append(runAsUser);
			}

			builder.Append(adbIntentArguments);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output.Any(l => l.StartsWith("Starting service:", StringComparison.OrdinalIgnoreCase));
		}

		public void ForceStop(string packageName, AdbToolSettings settings = null)
		{
			//force-stop package
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("force-stop");
			builder.Append(packageName);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void Kill(string packageName, string forUser = null, AdbToolSettings settings = null)
		{
			// kill[options] package
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("kill");
			builder.Append(packageName);

			if (!string.IsNullOrEmpty(forUser))
			{
				builder.Append("--user");
				builder.Append(forUser);
			}

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public void KillAll(AdbToolSettings settings = null)
		{
			// killall
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("killall");

			var output = new List<string>();
			RunAdb(settings, builder, out output);
		}

		public int Broadcast(string intent, string toUser = null, AdbToolSettings settings = null)
		{
			const string rxBroadcastResult = "Broadcast completed:\\s+result\\s?=\\s?(?<result>[0-9]+)";

			// broadcast [options] intent
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("broadcast");

			if (!string.IsNullOrEmpty(toUser))
			{
				builder.Append("--user");
				builder.Append(toUser);
			}

			builder.Append(intent);

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			foreach (var line in output)
			{
				var match = Regex.Match(line, rxBroadcastResult, RegexOptions.Singleline | RegexOptions.IgnoreCase);
				var r = match?.Groups?["result"]?.Value ?? "-1";
				var rInt = -1;
				if (int.TryParse(r, out rInt))
					return rInt;
			}

			return -1;
		}

		public List<string> Instrument(string component, AmInstrumentOptions options = null, AdbToolSettings settings = null)
		{
			// instrument [options] component
			if (settings == null)
				settings = new AdbToolSettings();
			if (options == null)
				options = new AmInstrumentOptions();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("instrument");

			if (options.PrintRawResults)
				builder.Append("-r");

			foreach (var kvp in options.KeyValues)
			{
				var v = string.Join(",", kvp.Value);
				builder.Append("-e {0} {1}", kvp.Key, v);
			}

			if (options.ProfileToFile != null)
			{
				builder.Append("-p");
				builder.AppendQuoted(options.ProfileToFile.MakeAbsolute(environment).FullPath);
			}

			if (options.Wait)
				builder.Append("-w");

			if (options.NoWindowAnimation)
				builder.Append("--no-window-animation");


			if (!string.IsNullOrEmpty(options.RunAsUser))
			{
				builder.Append("--user");
				builder.Append(options.RunAsUser);
			}

			builder.Append(component);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public List<string> StartProfiling(string process, FilePath outputFile, AdbToolSettings settings = null)
		{
			// broadcast [options] intent
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("profile");
			builder.Append("start");

			builder.Append(process);

			builder.AppendQuoted(outputFile.MakeAbsolute(environment).FullPath);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public List<string> StopProfiling(string process, AdbToolSettings settings = null)
		{
			// broadcast [options] intent
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("profile");
			builder.Append("stop");

			builder.Append(process);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public List<string> DumpHeap(string process, FilePath outputFile, string forUser = null, bool dumpNativeHeap = false, AdbToolSettings settings = null)
		{
			// dumpheap [options] process file
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("dumpheap");

			if (!string.IsNullOrEmpty(forUser))
			{
				builder.Append("--user");
				builder.Append(forUser);
			}

			if (dumpNativeHeap)
				builder.Append("-n");

			builder.Append(process);

			builder.AppendQuoted(outputFile.MakeAbsolute(environment).FullPath);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public List<string> SetDebugApp(string packageName, bool wait = false, bool persistent = false, AdbToolSettings settings = null)
		{
			// set-debug-app [options] package
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("set-debug-app");

			if (wait)
				builder.Append("-w");

			if (persistent)
				builder.Append("--persistent");

			builder.Append(packageName);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public List<string> ClearDebugApp(AdbToolSettings settings = null)
		{
			// clear-debug-app
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("clear-debug-app");

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public List<string> Monitor(int? gdbPort = null, AdbToolSettings settings = null)
		{
			// monitor [options]
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("monitor");

			if (gdbPort.HasValue)
				builder.Append("--gdb:" + gdbPort.Value);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public List<string> ScreenCompat(bool compatOn, string packageName, AdbToolSettings settings = null)
		{
			// screen-compat {on|off} package
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("screen-compat");

			builder.Append(compatOn ? "on" : "off");

			builder.Append(packageName);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}




		public List<string> DisplaySize(int width, int height, AdbToolSettings settings = null)
		{
			return displaySize(settings, false, width, height);
		}
		public List<string> ResetDisplaySize(AdbToolSettings settings = null)
		{
			return displaySize(settings, true, -1, -1);
		}
		List<string> displaySize(AdbToolSettings settings, bool reset, int width, int height)
		{
			// display-size [reset|widthxheight]
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("display-size");

			if (reset)
				builder.Append("reset");
			else
				builder.Append(string.Format("{0}x{1}", width, height));

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public List<string> DisplayDensity(int dpi, AdbToolSettings settings = null)
		{
			// display-density dpi
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("display-density");

			builder.Append(dpi.ToString());

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return output;
		}

		public string IntentToURI(string intent, AdbToolSettings settings = null)
		{
			// display-density dpi
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("to-uri");

			builder.Append(intent);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return string.Join(Environment.NewLine, output);
		}

		public string IntentToIntentURI(string intent, AdbToolSettings settings = null)
		{
			// display-density dpi
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("am");

			builder.Append("to-intent-uri");

			builder.Append(intent);

			var output = new List<string>();
			RunAdb(settings, builder, out output);
			return string.Join(Environment.NewLine, output);
		}
	}
}