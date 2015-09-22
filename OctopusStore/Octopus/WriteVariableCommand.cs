using System.Linq;
using Conifer;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusStore.Config;
using OctopusStore.Infrastructure;

namespace OctopusStore.Octopus
{
	public class WriteVariableCommand
	{
		private readonly IConfiguration _config;
		private readonly VariableFilter _filter;

		public WriteVariableCommand(IConfiguration config, VariableFilter filter)
		{
			_config = config;
			_filter = filter;
		}

		public void Execute(string key, string value)
		{
			var factory = new OctopusClientFactory();
			var client = factory.CreateClient(new OctopusServerEndpoint(_config.OctopusHost + "api", _config.OctopusApiKey));
			var repo = new OctopusRepository(client);

			var libararySet = repo.LibraryVariableSets.FindOne(vs => vs.Name == _config.VariableSetName);
			var variableSet = repo.VariableSets.Get(libararySet.VariableSetId);

			variableSet
				.Variables
				.Where(v => _filter.ShouldReturnVariable(variableSet.ScopeValues, v))
				.Where(v => v.Name.EqualsIgnore(key))
				.OrBlank(() => CreateVariable(key, variableSet))
				.ForEach(v => v.Value = value);

			repo.VariableSets.Modify(variableSet);
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
