using Octopus.Client.Model;

namespace OctopusStore.Config
{
	public interface IVaraibleFilter
	{
		bool ShouldReturnVariable(VariableScopeValues scopeValues, VariableResource variable);
	}
}
