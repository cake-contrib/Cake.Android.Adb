using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AndroidAdb
{
	partial class AdbTool
	{
		public List<AdbDeviceInfo> GetDevices(AdbToolSettings settings = null)
		{
			var devices = new List<AdbDeviceInfo>();

			if (settings == null)
				settings = new AdbToolSettings();

			//adb devices -l
			var builder = new ProcessArgumentBuilder();

			builder.Append("devices");
			builder.Append("-l");

			var p = RunProcess(settings, builder, new ProcessSettings
			{
				RedirectStandardOutput = true,
			});
			p.WaitForExit();

			foreach (var line in p.GetStandardOutput().Skip(1))
			{
				var parts = Regex.Split(line, "\\s+");

				var d = new AdbDeviceInfo
				{
					Serial = parts[0].Trim()
				};

				if (parts.Length > 1 && (parts[1]?.ToLowerInvariant() ?? "offline") == "offline")
					continue;

				if (parts.Length > 2)
				{
					foreach (var part in parts.Skip(2))
					{
						var bits = part.Split(new[] { ':' }, 2);
						if (bits == null || bits.Length != 2)
							continue;

						switch (bits[0].ToLower())
						{
							case "usb":
								d.Usb = bits[1];
								break;
							case "product":
								d.Product = bits[1];
								break;
							case "model":
								d.Model = bits[1];
								break;
							case "device":
								d.Device = bits[1];
								break;
						}
					}
				}

				if (!string.IsNullOrEmpty(d?.Serial))
					devices.Add(d);
			}

			return devices;
		}

		public void KillServer(AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			//adb kill-server
			var builder = new ProcessArgumentBuilder();

			builder.Append("kill-server");

			Run(settings, builder);
		}

		public void StartServer(AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			//adb kill-server
			var builder = new ProcessArgumentBuilder();

			builder.Append("start-server");

			Run(settings, builder);
		}

		public void Connect(string deviceIp, int port = 5555, AdbToolSettings settings = null)
		{
			// adb connect device_ip_address:5555
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			builder.Append("connect");
			builder.Append(deviceIp + ":" + port);

			Run(settings, builder);
		}

		public void Disconnect(string deviceIp = null, int? port = null, AdbToolSettings settings = null)
		{
			// adb connect device_ip_address:5555
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			builder.Append("disconnect");
			if (!string.IsNullOrEmpty(deviceIp))
				builder.Append(deviceIp + ":" + (port ?? 5555));

			Run(settings, builder);
		}

		public void Install(FilePath apkFile, AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("install");
			builder.Append(apkFile.MakeAbsolute(environment).FullPath);

			Run(settings, builder);
		}

		public void WaitFor(AdbTransport transport = AdbTransport.Any, AdbState state = AdbState.Device, AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb wait-for[-<transport>]-<state>
			//  transport: usb, local, or any (default)
			//  state: device, recovery, sideload, bootloader
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			var x = "wait-for";
			if (transport == AdbTransport.Local)
				x = "-local";
			else if (transport == AdbTransport.Usb)
				x = "-usb";

			switch (state)
			{
				case AdbState.Bootloader:
					x += "-bootloader";
					break;
				case AdbState.Device:
					x += "-device";
					break;
				case AdbState.Recovery:
					x += "-recovery";
					break;
				case AdbState.Sideload:
					x += "-sideload";
					break;
			}

			builder.Append(x);

			Run(settings, builder);
		}

		public void Uninstall(string packageName, bool keepDataAndCacheDirs = false, AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("uninstall");
			if (keepDataAndCacheDirs)
				builder.Append("-k");
			builder.Append(packageName);

			Run(settings, builder);
		}

		public bool EmuKill(AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("emu");
			builder.Append("kill");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return output != null && output.Any(o => o.ToLowerInvariant().Contains("stopping emulator")));
		}

		public List<string> Run(string[] args, AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			if (args != null && args.Length > 0)
			{
				foreach (var a in args)
					builder.Append(a);
			}

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return output;
		}

		public bool Pull(FilePath remoteFileSource, FilePath localFileDestination, AdbToolSettings settings = null)
		{
			return pull(settings, remoteFileSource.MakeAbsolute(environment).FullPath, localFileDestination.MakeAbsolute(environment).FullPath);
		}

		public bool Pull(DirectoryPath remoteDirectorySource, DirectoryPath localDirectoryDestination, AdbToolSettings settings = null)
		{
			return pull(settings, remoteDirectorySource.MakeAbsolute(environment).FullPath, localDirectoryDestination.MakeAbsolute(environment).FullPath);
		}

		public bool Pull(FilePath remoteFileSource, DirectoryPath localDirectoryDestination, AdbToolSettings settings = null)
		{
			return pull(settings, remoteFileSource.MakeAbsolute(environment).FullPath, localDirectoryDestination.MakeAbsolute(environment).FullPath);
		}

		bool pull(AdbToolSettings settings, string remoteSrc, string localDest)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("pull");
			builder.AppendQuoted(remoteSrc);
			builder.AppendQuoted(localDest);

			return RunAdb(settings, builder);
		}

		public bool Push(FilePath localFileSource, FilePath remoteFileDestination, AdbToolSettings settings = null)
		{
			return push(settings, localFileSource.MakeAbsolute(environment).FullPath, remoteFileDestination.MakeAbsolute(environment).FullPath);
		}

		public bool Push(FilePath localFileSource, DirectoryPath remoteDirectoryDestination, AdbToolSettings settings = null)
		{
			return push(settings, localFileSource.MakeAbsolute(environment).FullPath, remoteDirectoryDestination.MakeAbsolute(environment).FullPath);
		}

		public bool Push(DirectoryPath localDirectorySource, DirectoryPath remoteDirectoryDestination, AdbToolSettings settings = null)
		{
			return push(settings, localDirectorySource.MakeAbsolute(environment).FullPath, remoteDirectoryDestination.MakeAbsolute(environment).FullPath);
		}

		bool push(AdbToolSettings settings, string localSrc, string remoteDest)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("pull");
			builder.AppendQuoted(localSrc);
			builder.AppendQuoted(remoteDest);

			return RunAdb(settings, builder);
		}

		public List<string> BugReport(AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("bugreport");

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return output;
		}


		public List<string> Logcat(AdbLogcatOptions options = null, string filter = null, AdbToolSettings settings = null)
		{
			// logcat[option][filter - specs]

			if (settings == null)
				settings = new AdbToolSettings();
			if (options == null)
				options = new AdbLogcatOptions();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("logcat");

			if (options.BufferType != AdbLogcatBufferType.Main)
			{
				builder.Append("-b");
				builder.Append(options.BufferType.ToString().ToLowerInvariant());
			}

			if (options.Clear || options.PrintSize)
			{
				if (options.Clear)
					builder.Append("-c");
				else if (options.PrintSize)
					builder.Append("-g");
			}
			else
			{
				// Always dump, since we want to return and not listen to logcat forever
				// in the future might be nice to add an alias that takes a cancellation token
				// and can pipe output until that token is cancelled.
				//if (options.Dump)
				builder.Append("-d");

				if (options.OutputFile != null)
				{
					builder.Append("-f");
					builder.AppendQuoted(options.OutputFile.MakeAbsolute(environment).FullPath);

					if (options.NumRotatedLogs.HasValue)
					{
						builder.Append("-n");
						builder.Append(options.NumRotatedLogs.Value.ToString());
					}

					var kb = options.LogRotationKb ?? 16;
					builder.Append("-r");
					builder.Append(kb.ToString());
				}

				if (options.SilentFilter)
					builder.Append("-s");

				if (options.Verbosity != AdbLogcatOutputVerbosity.Brief)
				{
					builder.Append("-v");
					builder.Append(options.Verbosity.ToString().ToLowerInvariant());
				}

			}

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return output;
		}

		public string Version(AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb version
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("version");
			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return string.Join(Environment.NewLine, output);
		}

		public string GetSerialNumber(AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("get-serialno");
			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return string.Join(Environment.NewLine, output);
		}

		public string GetState(AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("get-state");
			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return string.Join(Environment.NewLine, output);
		}


		public List<string> Shell(string shellCommand, AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append(shellCommand);

			var output = new List<string>();
			RunAdb(settings, builder, out output);

			return output;
		}


		public void ScreenCapture(FilePath saveToLocalFile, AdbToolSettings settings = null)
		{
			if (settings == null)
				settings = new AdbToolSettings();

			//adb shell screencap / sdcard / screen.png
			var guid = Guid.NewGuid().ToString();
			var remoteFile = "/sdcard/" + guid + ".png";

			Shell("screencap " + remoteFile, settings);

			Pull(remoteFile, saveToLocalFile, settings);

			Shell("rm " + remoteFile, settings);
		}

		public void ScreenRecord(FilePath saveToLocalFile, System.Threading.CancellationToken? recordingCancelToken = null, TimeSpan? timeLimit = null, int? bitrateMbps = null, int? width = null, int? height = null, bool rotate = false, bool logVerbose = false, AdbToolSettings settings = null)
		{
			// screenrecord[options] filename

			if (settings == null)
				settings = new AdbToolSettings();

			var guid = Guid.NewGuid().ToString();
			var remoteFile = "/sdcard/" + guid + ".mp4";

			// adb uninstall -k <package>
			// -k keeps data & cache dir
			var builder = new ProcessArgumentBuilder();

			AddSerial(settings.Serial, builder);

			builder.Append("shell");
			builder.Append("screenrecord");

			if (timeLimit.HasValue)
			{
				builder.Append("--time-limit");
				builder.Append(((int)timeLimit.Value.TotalSeconds).ToString());
			}

			if (bitrateMbps.HasValue)
			{
				builder.Append("--bit-rate");
				builder.Append((bitrateMbps.Value * 1000000).ToString());
			}

			if (width.HasValue && height.HasValue)
			{
				builder.Append("--size");
				builder.Append("{0}x{1}", width, height);
			}

			if (rotate)
				builder.Append("--rotate");

			if (logVerbose)
				builder.Append("--verbose");

			builder.Append(remoteFile);

			var output = new List<string>();

			if (recordingCancelToken.HasValue)
				RunAdb(settings, builder, recordingCancelToken.Value, out output);
			else
				RunAdb(settings, builder, out output);

			Pull(remoteFile, saveToLocalFile, settings);

			Shell("rm " + remoteFile, settings);
		}
	}
}
