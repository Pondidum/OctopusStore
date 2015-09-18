using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
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
			_server = TestServer.Create(app =>
			{
				var config = new HttpConfiguration();
				WebApiConfig.Register(config);

				app.UseWebApi(config);
			});
		}

		private HttpResponseMessage Request(HttpMethod method, string url)
		{
			return _server
				.HttpClient
				.SendAsync(new HttpRequestMessage(method, url))
				.Result;
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
			Request(HttpMethod.Put, "v1/kv/web/key1").StatusCode.ShouldBe(HttpStatusCode.OK);
			Request(HttpMethod.Put, "v1/kv/web/key2?flags=42").StatusCode.ShouldBe(HttpStatusCode.OK);
			Request(HttpMethod.Put, "v1/kv/web/sub/key3").StatusCode.ShouldBe(HttpStatusCode.OK);

			var response = Request(HttpMethod.Get, "v1/kv/?recurse");
			var body = BodyOf<IEnumerable<ValueModel>>(response);

			body.ShouldBe(new[]
			{
				new ValueModel(), 
				new ValueModel(), 
				new ValueModel(), 
			});
		}

		public void Dispose()
		{
			_server.Dispose();
		}
	}
}