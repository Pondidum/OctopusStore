using System;
using System.Configuration;

namespace OctopusStore
{
	public class Configuration : IConfiguration
	{
		public Uri OctopusHost { get; }
		public string VariableSetName { get; }
		public string OctopusApiKey { get; }

		public Configuration()
		{
			OctopusHost = new Uri(ConfigurationManager.AppSettings["OctopusHost"]);
			OctopusApiKey = ConfigurationManager.AppSettings["OctopusApiKey"];
			VariableSetName = ConfigurationManager.AppSettings["VariableSetName"];
		}
	}
}
