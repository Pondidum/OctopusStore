using Octopus.Client.Model;

namespace OctopusStore
{
	public interface IVaraibleFilter
	{
		bool ShouldReturnVariable(VariableScopeValues scopeValues, VariableResource variable);
	}
}
