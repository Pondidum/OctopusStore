using System;
using System.Linq;
using OctopusStore.Config;
using OctopusStore.Infrastructure;

namespace OctopusStore.Octopus
{
	public class DeleteVariableCommand : CommandBase
	{
		private readonly VariableFilter _filter;

		public DeleteVariableCommand(IConfiguration config, VariableFilter filter) : base(config)
		{
			_filter = filter;
		}

		public void Execute(string key, bool recursive)
		{
			var variableSet = GetVariableSet();

			variableSet
				.Variables
				.Where(v => _filter.ShouldReturnVariable(variableSet.ScopeValues, v))
				.Where(v => recursive ? v.Name.StartsWith(key, StringComparison.OrdinalIgnoreCase) : v.Name.EqualsIgnore(key))
				.ToList()
				.ForEach(v => variableSet.Variables.Remove(v));

			Modify(variableSet);
		}
	}
}
