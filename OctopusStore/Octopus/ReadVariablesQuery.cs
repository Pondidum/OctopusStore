using System.Collections.Generic;
using System.Linq;
using OctopusStore.Config;

namespace OctopusStore.Octopus
{
	public class ReadVariablesQuery : CommandBase
	{
		private readonly VariableFilter _filter;

		public ReadVariablesQuery(IConfiguration config, VariableFilter filter) : base(config)
		{
			_filter = filter;
		}

		public IEnumerable<KeyValuePair<string, string>> Execute()
		{
			var variableSet = GetVariableSet();

			return variableSet
				.Variables
				.Where(v => _filter.ShouldReturnVariable(variableSet.ScopeValues, v))
				.Select(vs => new KeyValuePair<string, string>(vs.Name, vs.Value));
		}
	}
}
