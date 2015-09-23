using System.Linq;
using Octopus.Client;
using OctopusStore.Config;
using OctopusStore.Infrastructure;

namespace OctopusStore.Octopus
{
	public class DeleteVariableCommand
	{
		private readonly IConfiguration _config;
		private readonly VariableFilter _filter;

		public DeleteVariableCommand(IConfiguration config, VariableFilter filter)
		{
			_config = config;
			_filter = filter;
		}

		public void Execute(string key)
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
				.ToList()
				.ForEach(v => variableSet.Variables.Remove(v));

			repo.VariableSets.Modify(variableSet);
		}
	}
}
