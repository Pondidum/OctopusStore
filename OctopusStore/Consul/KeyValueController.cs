using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

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

			var recurse = pairs.Any(pair => pair.Key.Equals("recurse", StringComparison.OrdinalIgnoreCase));

			return recurse
				? _store.GetValuesPrefixed(key)
				: _store.GetValue(key);
		}

		public HttpResponseMessage PutKv([FromBody]string content, string keyGreedy)
		{
			_store.WriteValue(keyGreedy, model =>
			{
				model.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));

				//if request has "?flags" then write them
			});

			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
