using System;
using System.Collections.Generic;
using System.Linq;
using OctopusStore.Consul;
using OctopusStore.Infrastructure;

namespace OctopusStore.Octopus
{
	public class VariableBackingStore : IVariableBackingStore
	{
		private readonly GetVariablesQuery _query;

		public VariableBackingStore(GetVariablesQuery query)
		{
			_query = query;
		}

		public void Write(ValueModel model)
		{

		}

		public ValueModel Read(string key)
		{
			return _query
				.Execute()
				.Where(pair => pair.Key.EqualsIgnore(key))
				.Select(m => new ValueModel { Key = m.Key, Value = m.Value.ToBase64() })
				.FirstOrDefault();
		}

		public IEnumerable<ValueModel> ReadPrefixed(string keyPrefix)
		{
			return _query
				.Execute()
				.Where(pair => pair.Key.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase))
				.Select(m => new ValueModel { Key = m.Key, Value = m.Value.ToBase64() });
		}
	}
}
