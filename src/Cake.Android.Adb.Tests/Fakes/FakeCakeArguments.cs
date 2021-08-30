using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;

namespace Cake.AndroidAdb.Fakes
{
	internal sealed class FakeCakeArguments : ICakeArguments
	{
		readonly Dictionary<string, List<string>> _arguments;

		public FakeCakeArguments()
		{
			_arguments = new Dictionary<string, List<string>>();
		}
		public ICollection<string> GetArguments(string name)
			=> _arguments?[name] ?? new List<string>();

		public IDictionary<string, ICollection<string>> GetArguments()
			=> new Dictionary<string, ICollection<string>>(_arguments.Select(kvp => new KeyValuePair<string, ICollection<string>>(kvp.Key, kvp.Value)));

		public bool HasArgument(string name)
		{
			throw new NotImplementedException();
		}
	}
}