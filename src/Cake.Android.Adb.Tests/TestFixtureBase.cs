using Cake.Core;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cake.AndroidAdb.Fakes
{
	public abstract class TestFixtureBase : IDisposable
	{
		FakeCakeContext context;

		public ICakeContext Cake { get { return context.CakeContext; } }

		protected static string ContentPath
			=> System.IO.Path.GetDirectoryName(typeof(TestFixtureBase).Assembly.Location);

		public TestFixtureBase()
		{
			Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(TestFixtureBase).Assembly.Location);
			context = new FakeCakeContext();
		}

		public void Dispose()
		{
			//context.DumpLogs();
		}
	}
}
