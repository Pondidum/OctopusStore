using System.Web.Http.Dependencies;
using StructureMap;

namespace OctopusStore
{
	public class StructureMapDependencyResolver : StructureMapDependencyScope, IDependencyResolver
	{
		public StructureMapDependencyResolver(IContainer container)
			: base(container)
		{
		}

		public IDependencyScope BeginScope()
		{
			var child = Container.GetNestedContainer();
			return new StructureMapDependencyScope(child);
		}
	}
}
