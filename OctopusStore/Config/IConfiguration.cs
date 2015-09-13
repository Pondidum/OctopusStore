using System;

namespace OctopusStore.Config
{
	public interface IConfiguration
	{
		Uri OctopusHost { get; }
		string VariableSetName { get; }
		string OctopusApiKey { get; }
		FilterConfiguration Filter { get; }
	}
}
