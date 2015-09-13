using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;

namespace OctopusStore.Config
{
	public class VariableFilter : IVaraibleFilter
	{
		private readonly FilterConfiguration _filter;

		public VariableFilter(IConfiguration config)
		{
			_filter = config.Filter;
		}

		public bool ShouldReturnVariable(VariableScopeValues scopeValues, VariableResource variable)
		{
			var environments = MapScope(scopeValues.Environments, _filter.Environments);
			var roles = MapScope(scopeValues.Roles, _filter.Roles);
			var targets = MapScope(scopeValues.Machines, _filter.Targets);

			var scopes = new Dictionary<ScopeField, ScopeValue>();
			
			scopes[ScopeField.Environment] = new ScopeValue(environments);
			scopes[ScopeField.Role] = new ScopeValue(roles);
			scopes[ScopeField.Machine] = new ScopeValue(targets);

			return variable
				.Scope
				.IsExcludedBy(scopes) == false;
		}

		private static IEnumerable<string> MapScope(IEnumerable<ReferenceDataItem> collection, IEnumerable<string> config)
		{
			return collection.Join(
				config,
				s => s.Name,
				r => r,
				(s, r) => s.Id,
				StringComparer.OrdinalIgnoreCase);
		} 
	}
}
