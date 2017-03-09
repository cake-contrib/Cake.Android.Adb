using Cake.Core;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Path = System.IO.Path;

namespace Cake.AndroidAdb.Fakes
{
	[TestFixture]
	public abstract class TestFixtureBase
	{
		FakeCakeContext context;
	    private const string SDK_ROOT = "../../../android-sdk";
	    private static string ANDROID_HOME = System.Environment.GetEnvironmentVariable("ANDROID_HOME");


	    public ICakeContext Cake { get { return context.CakeContext; } }

		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Environment.CurrentDirectory = Path.GetDirectoryName(typeof(TestFixtureBase).Assembly.Location);
		}

		[SetUp]
		public void Setup ()
		{
			context = new FakeCakeContext();

			//var dp = new DirectoryPath("./testdata");
			//var d = context.CakeContext.FileSystem.GetDirectory(dp);

			//if (d.Exists)
			//	d.Delete(true);

			//d.Create();
		}

		[TearDown]
		public void Teardown()
		{
			//context.DumpLogs();
		}

	    protected AdbToolSettings GetAdbToolSettings()
		{
		    return Directory.Exists(ANDROID_HOME) ? new AdbToolSettings { SdkRoot = ANDROID_HOME } : new AdbToolSettings { SdkRoot = SDK_ROOT };
		}
	}
}
