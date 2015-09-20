using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace OctopusStore
{
	public class NullAsNotFoundFilter : ActionFilterAttribute
	{
		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			object outValue;
			actionExecutedContext.Response.TryGetContentValue(out outValue);

			if (outValue == null && actionExecutedContext.Request.Method == HttpMethod.Get)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			base.OnActionExecuted(actionExecutedContext);
		}

	}
}
