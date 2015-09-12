﻿using System;
using System.Linq;
using NSubstitute;
using Shouldly;
using Xunit;

namespace OctopusStore.Tests
{
	public class AcceptanceTests
	{
		[Fact(Skip = "Requires an OctopusServer to run")]
		public void When_querying_an_octopus_api()
		{
			var config = Substitute.For<IConfiguration>();
			config.OctopusHost.Returns(new Uri("http://172.28.128.20"));
			config.OctopusApiKey.Returns("API-F6LZ4DWCNSDVWNSXVIOIMA11S");
			config.VariableSetName.Returns("ConsulSet");

			var query = new GetVariablesQuery(config);
			query.Execute().ShouldNotBeEmpty();
		} 
	}
}