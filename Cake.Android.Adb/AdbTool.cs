using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AndroidAdb
{
	public partial class AdbTool : Tool<AdbToolSettings>
	{
		public AdbTool(ICakeContext cakeContext, IFileSystem cakeFileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IToolLocator toolLocator)
			: base(cakeFileSystem, cakeEnvironment, processRunner, toolLocator)
		{
			context = cakeContext;
			//fileSystem = cakeFileSystem;
			environment = cakeEnvironment;
		}

		//IFileSystem fileSystem;
		ICakeContext context;
		ICakeEnvironment environment;

		protected override string GetToolName()
		{
			return "adb";
		}

		protected override IEnumerable<string> GetToolExecutableNames()
		{
			return new List<string> {
				"adb",
				"adb.exe",
			};
		}

		protected override IEnumerable<FilePath> GetAlternativeToolPaths(AdbToolSettings settings = null)
		{
			var results = new List<FilePath>();

			var ext = environment.Platform.IsUnix() ? "" : ".exe";
			var androidHome = settings.SdkRoot.MakeAbsolute(environment).FullPath;

			if (!System.IO.Directory.Exists(androidHome))
				androidHome = environment.GetEnvironmentVariable("ANDROID_HOME");
			
			if (!string.IsNullOrEmpty(androidHome) && System.IO.Directory.Exists(androidHome))
			{
				var exe = new DirectoryPath(androidHome).Combine("platform-tools").CombineWithFilePath("adb" + ext);
				results.Add(exe);
			}

			return results;
		}

		void AddSerial(string serial, ProcessArgumentBuilder builder)
		{
			if (!string.IsNullOrEmpty(serial))
			{
				builder.Append("-s");
				builder.AppendQuoted(serial);
			}
		}

		bool RunAdb(AdbToolSettings settings, ProcessArgumentBuilder builder)
		{
			var output = new List<string>();
			return RunAdb(settings, builder, out output);
		}

		bool RunAdb(AdbToolSettings settings, ProcessArgumentBuilder builder, out List<string> output)
		{
			return RunAdb(settings, builder, System.Threading.CancellationToken.None, out output);
		}

		bool RunAdb(AdbToolSettings settings, ProcessArgumentBuilder builder, System.Threading.CancellationToken cancelToken, out List<string> output)
		{
			var p = RunProcess(settings, builder, new ProcessSettings
			{
				RedirectStandardOutput = true,
			});


			if (cancelToken != System.Threading.CancellationToken.None)
			{
				cancelToken.Register(() => {
					try { p.Kill(); }
					catch { }
				});
			}
			p.WaitForExit();

			output = p.GetStandardOutput().ToList();

			// Log out the lines anyway
			foreach (var line in output)
				context.Log.Write(Core.Diagnostics.Verbosity.Verbose, Core.Diagnostics.LogLevel.Information, line);
			
			return p.GetExitCode() == 0;
		}
	}
}
