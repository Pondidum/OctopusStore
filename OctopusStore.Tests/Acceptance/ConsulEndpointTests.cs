using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NSubstitute;
using OctopusStore.Config;
using OctopusStore.Consul;
using Owin;
using Shouldly;
using Xunit;

namespace OctopusStore.Tests
{
	public class ConsulEndpointTests : IDisposable
	{
		private readonly TestServer _server;

		//https://www.consul.io/intro/getting-started/kv.html
		public ConsulEndpointTests()
		{
			var config = Substitute.For<IConfiguration>();
			config.OctopusHost.Returns(new Uri("http://172.28.128.20"));
			config.OctopusApiKey.Returns("API-F6LZ4DWCNSDVWNSXVIOIMA11S");
			config.VariableSetName.Returns("UnitTestSet");
			config.Filter.Returns(new FilterConfiguration());
			config.Filter.Environments.Add("dev");

			_server = TestServer.Create(app =>
			{
				var http = new HttpConfiguration();
				WebApiConfig.Register(http, config);

				app.UseWebApi(http);
			});
		}

		private HttpResponseMessage Request(HttpMethod method, string url)
		{
			return _server
				.HttpClient
				.SendAsync(new HttpRequestMessage(method, url))
				.Result;
		}

		private void Put(string url, string body)
		{
			var result = _server
				.HttpClient
				.PutAsync(url, new StringContent(body))
				.Result;

			result
				.StatusCode
				.ShouldBe(HttpStatusCode.OK);
		}

		private T BodyOf<T>(HttpResponseMessage response)
		{
			var json = response
				.Content
				.ReadAsStringAsync()
				.Result;

			return JsonConvert.DeserializeObject<T>(json);
		}

		[Fact]
		public void When_there_are_no_keys()
		{
			Request(HttpMethod.Get, "v1/kv/?recurse").StatusCode.ShouldBe(HttpStatusCode.NotFound);
		}

		[Fact]
		public void When_getting_keys_after_putting()
		{
			Put("v1/kv/web/key1", "test");
			Put("v1/kv/web/key2?flags=42", "test");
			Put("v1/kv/web/sub/key3", "test");

			var response = Request(HttpMethod.Get, "v1/kv/?recurse");
			var body = BodyOf<List<ValueModel>>(response);

			var val1 = body[0];
			var val2 = body[1];
			var val3 = body[2];

			body.ShouldSatisfyAllConditions(
				() => val1.Key.ShouldBe("web/key1"),
				() => val2.Key.ShouldBe("web/key2"),
				() => val3.Key.ShouldBe("web/sub/key3"),
				() => val1.Value.ShouldBe("dGVzdA=="),
				() => val2.Value.ShouldBe("dGVzdA=="),
				() => val3.Value.ShouldBe("dGVzdA==")
			);
		}

		[Fact]
		public void When_getting_a_single_key()
		{
			Put("v1/kv/web/key1", "test");

			var response = Request(HttpMethod.Get, "v1/kv/web/key1");
			var body = BodyOf<IEnumerable<ValueModel>>(response);

			var val1 = body.Single();

			body.ShouldSatisfyAllConditions(
				() => val1.Key.ShouldBe("web/key1"),
				() => val1.Value.ShouldBe("dGVzdA==")
			);
		}

		[Fact]
		public void When_deleting_a_key()
		{
			Request(HttpMethod.Delete, "v1/kv/web/key1").StatusCode.ShouldBe(HttpStatusCode.OK);
		}

		public void Dispose()
		{
			_server.Dispose();
		}
	}
}