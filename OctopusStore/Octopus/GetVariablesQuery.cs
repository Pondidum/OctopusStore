using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using OctopusStore.Config;

namespace OctopusStore.Octopus
{
	public class GetVariablesQuery
	{
		private readonly IConfiguration _config;
		private readonly IVaraibleFilter _filter;

		public GetVariablesQuery(IConfiguration config, IVaraibleFilter filter)
		{
			_config = config;
			_filter = filter;
		}

		public IEnumerable<KeyValuePair<string, string>> Execute()
		{
			var factory = new OctopusClientFactory();
			var client = factory.CreateClient(new OctopusServerEndpoint(_config.OctopusHost + "api", _config.OctopusApiKey));
			var repo = new OctopusRepository(client);

			var libararySet = repo.LibraryVariableSets.FindOne(vs => vs.Name == _config.VariableSetName);
			var variableSet = repo.VariableSets.Get(libararySet.VariableSetId);

			return variableSet
				.Variables
				.Where(v => _filter.ShouldReturnVariable(variableSet.ScopeValues, v))
				.Select(vs => new KeyValuePair<string, string>(vs.Name, vs.Value));
		}
	}
}
