using System;
using System.Collections;
using System.Collections.Generic;
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

namespace OctopusStore.Tests.Acceptance
{
	public class GettingStarted
	{
		private TestServer _server;

		public GettingStarted()
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

		private void Put(string url, string body, HttpStatusCode code, bool returnValue)
		{
			var result = _server
				.HttpClient
				.PutAsync(url, new StringContent(body))
				.Result;

			result
				.StatusCode
				.ShouldBe(code);

			var content = result
				.Content
				.ReadAsStringAsync()
				.Result;

			Convert.ToBoolean(content).ShouldBe(returnValue);
		}

		private void Get(string url, HttpStatusCode code)
		{
			var result = _server
				.HttpClient
				.GetAsync(url)
				.Result;

			result.StatusCode.ShouldBe(code);
		}

		private void Get(string url, HttpStatusCode code, Action<IEnumerable<ValueModel>> validate)
		{
			var result = _server
				.HttpClient
				.GetAsync(url)
				.Result;

			result.StatusCode.ShouldBe(code);

			var json = result
				.Content
				.ReadAsStringAsync()
				.Result;

			validate(JsonConvert.DeserializeObject<IEnumerable<ValueModel>>(json));
		}

		private void Delete(string url, HttpStatusCode code)
		{
			var result = _server
				.HttpClient
				.DeleteAsync(url)
				.Result;

			result.StatusCode.ShouldBe(code);
		}

		//[Fact]
		public void Complete_run_through()
		{
			Get("v1/kv/?recurse", HttpStatusCode.NotFound);


			Put("v1/kv/web/key1", "test", HttpStatusCode.OK, true);
			Put("v1/kv/web/key2?flags=42", "test", HttpStatusCode.OK, true);
			Put("v1/kv/web/sub/key3", "test", HttpStatusCode.OK, true);

			Get("v1/kv/?recurse", HttpStatusCode.OK, body => body.ShouldBe(new[]
			{
				new ValueModel(),
				new ValueModel(),
				new ValueModel()
			}));

			Get("v1/kv/web/key1", HttpStatusCode.OK, body => body.ShouldBe(new[]
			{
				new ValueModel()
			}));


			Delete("v1/kv/web/sub?recurse", HttpStatusCode.OK);
			Get("v1/kv/web?recurse", HttpStatusCode.OK, body => body.ShouldBe(new[]
			{
				new ValueModel(),
				new ValueModel()
			}));

			Put("v1/kv/web/key1?cas=97", "newval", HttpStatusCode.OK, true);
			Put("v1/kv/web/key1?cas=97", "newval", HttpStatusCode.OK, false);

			Get("v1/kv/web/key2?index=101&wait=5s", HttpStatusCode.OK, body => body.ShouldBe(new[]
			{
				new ValueModel()
			}));
		}
	}
}
