using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OctopusStore.Consul
{
	public class KeyValueController : ApiController
	{
		public IEnumerable<ValueModel> GetKv(string keyGreedy)
		{
			return Enumerable.Empty<ValueModel>();
		}

		public HttpResponseMessage PutKv(string keyGreedy)
		{
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
