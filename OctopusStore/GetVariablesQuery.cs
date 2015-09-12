using System.Collections.Generic;
using System.Linq;
using Octopus.Client;

namespace OctopusStore
{
	public class GetVariablesQuery
	{
		private readonly IConfiguration _config;

		public GetVariablesQuery(IConfiguration config)
		{
			_config = config;
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
				.Select(vs => new KeyValuePair<string, string>(vs.Name, vs.Value));
		}
	}
}
