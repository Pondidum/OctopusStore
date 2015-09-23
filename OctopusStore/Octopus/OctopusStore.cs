using System;
using System.Collections.Generic;
using System.Linq;
using OctopusStore.Consul;
using OctopusStore.Infrastructure;

namespace OctopusStore.Octopus
{
	public class VariableBackingStore : IVariableBackingStore
	{
		private readonly ReadVariablesQuery _readQuery;
		private readonly WriteVariableCommand _writeCommand;
		private readonly DeleteVariableCommand _deleteCommand;

		public VariableBackingStore(ReadVariablesQuery readQuery, WriteVariableCommand writeCommand, DeleteVariableCommand deleteCommand)
		{
			_readQuery = readQuery;
			_writeCommand = writeCommand;
			_deleteCommand = deleteCommand;
		}

		public void Write(ValueModel model)
		{
			_writeCommand.Execute(model.Key, model.Value);
		}

		public ValueModel Read(string key)
		{
			return _readQuery
				.Execute()
				.Where(pair => pair.Key.EqualsIgnore(key))
				.Select(m => new ValueModel { Key = m.Key, Value = m.Value })
				.FirstOrDefault();
		}

		public IEnumerable<ValueModel> ReadPrefixed(string keyPrefix)
		{
			return _readQuery
				.Execute()
				.Where(pair => pair.Key.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase))
				.Select(m => new ValueModel { Key = m.Key, Value = m.Value });
		}

		public void Delete(string key)
		{
			_deleteCommand.Execute(key, false);
		}

		public void DeletePrefixed(string key)
		{
			_deleteCommand.Execute(key, true);
		}
	}
}
