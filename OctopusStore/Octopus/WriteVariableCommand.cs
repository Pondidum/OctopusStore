using System.Linq;
using Conifer;
using Octopus.Client.Model;
using OctopusStore.Config;
using OctopusStore.Infrastructure;

namespace OctopusStore.Octopus
{
	public class WriteVariableCommand : CommandBase
	{
		private readonly IConfiguration _config;
		private readonly VariableFilter _filter;

		public WriteVariableCommand(IConfiguration config, VariableFilter filter) : base(config)
		{
			_config = config;
			_filter = filter;
		}

		public void Execute(string key, string value)
		{
			var variableSet = GetVariableSet();
			
			variableSet
				.Variables
				.Where(v => _filter.ShouldReturnVariable(variableSet.ScopeValues, v))
				.Where(v => v.Name.EqualsIgnore(key))
				.OrBlank(() => CreateVariable(key, variableSet))
				.ForEach(v => v.Value = value);

			Modify(variableSet);
		}

		private VariableResource CreateVariable(string key, VariableSetResource variableSet)
		{
			var scope = new ScopeSpecification();
			scope.AddRange(VariableFilter.BuildScopeMap(variableSet.ScopeValues, _config.Filter));

			var v = new VariableResource
			{
				Name = key,
				Scope = scope
			};

			variableSet.Variables.Add(v);

			return v;
		}
	}
}
