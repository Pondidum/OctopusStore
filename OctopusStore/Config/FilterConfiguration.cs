using System.Collections.Generic;

namespace OctopusStore.Config
{
	public class FilterConfiguration
	{
		public List<string> Environments { get; set; }
		public List<string> Roles { get; set; }
		public List<string> Targets { get; set; }

		public FilterConfiguration()
		{
			Environments = new List<string>();
			Roles = new List<string>();
			Targets = new List<string>();
		}
	}
}
