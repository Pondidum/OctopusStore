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
		public void When_reading_from_octopus()
		{
			var query = new ReadVariablesQuery(_config, _filter);
			query.Execute().ShouldNotBeEmpty();
		}

		[Fact(Skip = "Requires an OctopusServer to run")]
		public void When_writing_to_octopus()
		{
			var command = new WriteVariableCommand(_config, _filter);
			command.Execute("newKey", "newValue");
		}

		[Fact(Skip = "Requires an OctopusServer to run")]
		public void When_deleting_from_octopus()
		{
			var command = new DeleteVariableCommand(_config, _filter);
			command.Execute("newKey", false);
		}
	}
}
