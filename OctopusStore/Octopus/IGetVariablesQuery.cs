using System.Collections.Generic;

namespace OctopusStore.Octopus
{
	public interface IGetVariablesQuery
	{
		IEnumerable<KeyValuePair<string, string>> Execute();
	}
}
