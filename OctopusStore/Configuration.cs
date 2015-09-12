using System;
using System.Configuration;
using Newtonsoft.Json;

namespace OctopusStore
{
	public class Configuration : IConfiguration
	{
		public Uri OctopusHost { get; }
		public string VariableSetName { get; }
		public string OctopusApiKey { get; }
		public FilterConfiguration Filter { get; }

		public Configuration()
		{
			OctopusHost = new Uri(ConfigurationManager.AppSettings["OctopusHost"]);
			OctopusApiKey = ConfigurationManager.AppSettings["OctopusApiKey"];
			VariableSetName = ConfigurationManager.AppSettings["VariableSetName"];
			Filter = JsonConvert.DeserializeObject<FilterConfiguration>(ConfigurationManager.AppSettings["Filter"]);
		}
	}
}
