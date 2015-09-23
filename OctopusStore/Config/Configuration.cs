using System;
using System.Configuration;
using Newtonsoft.Json;

namespace OctopusStore.Config
{
	public class Configuration : IConfiguration
	{
		public Uri OctopusHost { get; private set; }
		public string VariableSetName { get; private set; }
		public string OctopusApiKey { get; private set; }
		public FilterConfiguration Filter { get; private set; }

		public Configuration()
		{
			OctopusHost = new Uri(ConfigurationManager.AppSettings["OctopusHost"]);
			OctopusApiKey = ConfigurationManager.AppSettings["OctopusApiKey"];
			VariableSetName = ConfigurationManager.AppSettings["VariableSetName"];
			Filter = JsonConvert.DeserializeObject<FilterConfiguration>(ConfigurationManager.AppSettings["Filter"]);
		}
	}
}
