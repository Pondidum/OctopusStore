using System.Web.Http;
using Conifer;
using Conifer.Conventions;
using OctopusStore.Config;
using OctopusStore.Consul;
using StructureMap;
using StructureMap.Graph;

namespace OctopusStore
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration http)
		{
			var router = new Router(http, e =>
			{
				e.DefaultConventionsAre(new IRouteConvention[]
				{
					new SpecifiedPartRouteConvention("v1"),
					new MethodNameRouteConvention(),
					new ParameterNameRouteConvention().DetectGreedyArguments()
				});

				e.AddAll<KeyValueController>();
			});

			var container = new Container(config =>
			{
				config.Scan(a =>
				{
					a.TheCallingAssembly();
					a.WithDefaultConventions();
				});

				config.For<IConfiguration>().Use<Configuration>().Singleton();
			});

			http.DependencyResolver = new StructureMapDependencyResolver(container);
		}
	}
}
