using System;

namespace OctopusStore
{
	public interface IConfiguration
	{
		Uri OctopusHost { get; }
		string VariableSetName { get; }
	}
}
