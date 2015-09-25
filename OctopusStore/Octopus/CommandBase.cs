using Octopus.Client;
using Octopus.Client.Model;
using OctopusStore.Config;

namespace OctopusStore.Octopus
{
	public class CommandBase
	{
		private readonly IConfiguration _config;
		private readonly OctopusRepository _repo;

		public CommandBase(IConfiguration config)
		{
			_config = config;

			var factory = new OctopusClientFactory();
			var client = factory.CreateClient(new OctopusServerEndpoint(_config.OctopusHost + "api", _config.OctopusApiKey));
			_repo = new OctopusRepository(client);
		}

		protected VariableSetResource GetVariableSet()
		{
			var libararySet = _repo.LibraryVariableSets.FindOne(vs => vs.Name == _config.VariableSetName);

			return _repo.VariableSets.Get(libararySet.VariableSetId);
		}

		protected void Modify(VariableSetResource variableSet)
		{
			_repo.VariableSets.Modify(variableSet);
		}
	}
}
