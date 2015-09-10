using System;
using System.Configuration;

namespace OctopusStore
{
	public class Configuration : IConfiguration
	{
		public Uri OctopusHost { get; }

		public Configuration()
		{
			OctopusHost = new Uri(ConfigurationManager.AppSettings["OctopusHost"]);
		}
	}
}
