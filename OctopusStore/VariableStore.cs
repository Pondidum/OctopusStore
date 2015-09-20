using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OctopusStore.Consul;
using OctopusStore.Infrastructure;

namespace OctopusStore
{
	public class VariableStore
	{
		private int _offset;
		private readonly List<ValueModel> _store;

		public VariableStore()
		{
			_offset = 100;
			_store = new List<ValueModel>();
		}

		public IEnumerable<ValueModel> GetValue(string key)
		{
			return _store.Where(model => model.Key.EqualsIgnore(key));
		}

		public IEnumerable<ValueModel> GetValuesPrefixed(string keyPrefix)
		{
			return _store.Where(model => model.Key.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase));
		}

		public void WriteValue(string key, Action<ValueModel> bind)
		{
			var index = Interlocked.Increment(ref _offset);
			var model = _store.FirstOrDefault(m => m.Key.EqualsIgnore(key));

			if (model == null)
			{
				model = new ValueModel();
				_store.Add(model);
			}

			var create = model.CreateIndex ?? index;
			var modify = index;

			bind(model);

			model.Key = key;
			model.CreateIndex = create;
			model.ModifyIndex = modify;
		}

	}
}
