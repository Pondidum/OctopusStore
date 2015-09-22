using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;

namespace OctopusStore.Config
{
	public class VariableFilter
	{
		private readonly FilterConfiguration _filter;

		public VariableFilter(IConfiguration config)
		{
			_filter = config.Filter;
		}

		public bool ShouldReturnVariable(VariableScopeValues scopeValues, VariableResource variable)
		{
			var scopes = BuildScopeMap(scopeValues, _filter);

			return variable
				.Scope
				.IsExcludedBy(scopes) == false;
		}

		public static Dictionary<ScopeField, ScopeValue> BuildScopeMap(VariableScopeValues scopeValues, FilterConfiguration filter)
		{
			var environments = MapScope(scopeValues.Environments, filter.Environments);
			var roles = MapScope(scopeValues.Roles, filter.Roles);
			var targets = MapScope(scopeValues.Machines, filter.Targets);

			return new Dictionary<ScopeField, ScopeValue>
			{
				[ScopeField.Environment] = new ScopeValue(environments),
				[ScopeField.Role] = new ScopeValue(roles),
				[ScopeField.Machine] = new ScopeValue(targets)
			};
		}

		public static IEnumerable<string> MapScope(IEnumerable<ReferenceDataItem> collection, IEnumerable<string> config)
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
