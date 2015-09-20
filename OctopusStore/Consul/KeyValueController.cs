﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using OctopusStore.Infrastructure;

namespace OctopusStore.Consul
{
	public class KeyValueController : ApiController
	{
		private readonly VariableStore _store;

		public KeyValueController(VariableStore store)
		{
			_store = store;
		}

		public IEnumerable<ValueModel> GetKv(string keyGreedy)
		{
			var key = keyGreedy ?? string.Empty;
			var pairs = Request.GetQueryNameValuePairs();

			var recurse = pairs.Any(pair => pair.Key.EqualsIgnore("recurse"));

			return recurse
				? _store.GetValuesPrefixed(key)
				: _store.GetValue(key);
		}

		public HttpResponseMessage PutKv([FromBody]string content, string keyGreedy)
		{
			var pairs = Request.GetQueryNameValuePairs();

			_store.WriteValue(keyGreedy, model =>
			{
				model.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));

				pairs
					.Where(p => p.Key.EqualsIgnore("flags"))
					.Select(p => Convert.ToInt32(p.Value))
					.DoFirst(value => model.Flags = value);
			});

			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
