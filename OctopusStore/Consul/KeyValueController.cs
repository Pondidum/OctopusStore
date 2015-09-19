using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OctopusStore.Consul
{
	public class KeyValueController : ApiController
	{
		public HttpResponseMessage GetKv(string keyGreedy)
		{
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		public HttpResponseMessage PutKv(string keyGreedy)
		{
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
