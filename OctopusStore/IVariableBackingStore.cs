using System.Collections.Generic;
using OctopusStore.Consul;

namespace OctopusStore
{
	public interface IVariableBackingStore
	{
		void Write(ValueModel model);
		ValueModel Read(string key);
		IEnumerable<ValueModel> ReadPrefixed(string keyPrefix);

		void Delete(string key);
		void DeletePrefixed(string key);
	}
}
