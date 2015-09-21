using System;
using System.Collections.Generic;
using System.Threading;
using OctopusStore.Consul;

namespace OctopusStore
{
	public class VariableStore
	{
		private readonly IVariableBackingStore _backingStore;
		private int _offset;

		public VariableStore(IVariableBackingStore backingStore)
		{
			_backingStore = backingStore;
			_offset = 100;
		}

		public IEnumerable<ValueModel> GetValue(string key)
		{
			return new []
			{
				_backingStore.Read(key)
			};
		}

		public IEnumerable<ValueModel> GetValuesPrefixed(string keyPrefix)
		{
			return _backingStore.ReadPrefixed(keyPrefix);
		}

		public void WriteValue(string key, Action<ValueModel> bind)
		{
			var index = Interlocked.Increment(ref _offset);
			var model = _backingStore.Read(key) ?? new ValueModel();

			var create = model.CreateIndex ?? index;
			var modify = index;

			bind(model);

			model.Key = key;
			model.CreateIndex = create;
			model.ModifyIndex = modify;

			_backingStore.Write(model);
		}

	}
}
