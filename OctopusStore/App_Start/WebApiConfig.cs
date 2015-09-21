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
			Register(http, new Configuration());
		}

		public static void Register(HttpConfiguration http, IConfiguration config)
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

			var container = new Container(c =>
			{
				c.Scan(a =>
				{
					a.TheCallingAssembly();
					a.WithDefaultConventions();
				});

				c.For<IConfiguration>().Use(config);
				c.For<VariableStore>().Use<VariableStore>().Singleton();
			});

			http.Filters.Add(new NullAsNotFoundFilter());
			http.Formatters.Add(new PlainTextMediaTypeFormatter());
			http.DependencyResolver = new StructureMapDependencyResolver(container);
		}
	}
}
