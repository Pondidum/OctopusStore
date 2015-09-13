using System.Web.Http;
using Conifer;
using Conifer.Conventions;
using OctopusStore.Consul;

namespace OctopusStore
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var router = new Router(config, e =>
			{
				e.DefaultConventionsAre(new IRouteConvention[]
				{
					new SpecifiedPartRouteConvention("v1"),
					new MethodNameRouteConvention(),
					new ParameterNameRouteConvention()
				});

				e.AddAll<KeyValueController>();
			});
		}
	}
}
