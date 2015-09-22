using System;
using NSubstitute;
using OctopusStore.Config;
using OctopusStore.Octopus;
using Shouldly;
using Xunit;

namespace OctopusStore.Tests
{
	public class OctopusIntegrationTests
	{
		private readonly IConfiguration _config;
		private readonly VariableFilter _filter;

		public OctopusIntegrationTests()
		{
			var filterConfig = new FilterConfiguration
			{
				Environments = { "dev" }
			};

			_config = Substitute.For<IConfiguration>();
			_config.OctopusHost.Returns(new Uri("http://172.28.128.20"));
			_config.OctopusApiKey.Returns("API-F6LZ4DWCNSDVWNSXVIOIMA11S");
			_config.VariableSetName.Returns("ConsulSet");
			_config.Filter.Returns(filterConfig);

			_filter = new VariableFilter(_config);
		}

		[Fact(Skip = "Requires an OctopusServer to run")]
		public void When_querying_an_octopus_api()
		{
			var query = new GetVariablesQuery(_config, _filter);
			query.Execute().ShouldNotBeEmpty();
		}

		[Fact(Skip = "Requires an OctopusServer to run")]
		public void FactMethodName()
		{
			var command = new WriteVariableCommand(_config, _filter);
			command.Execute("newKey", "newValue");
		}
	}
}
