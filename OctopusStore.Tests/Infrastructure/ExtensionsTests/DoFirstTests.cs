using OctopusStore.Infrastructure;
using Shouldly;
using Xunit;

namespace OctopusStore.Tests.Infrastructure.ExtensionsTests
{
	public class DoFirstTests
	{
		[Fact]
		public void When_running_on_an_empty_collection()
		{
			var collection = new Model[] { };

			Should.NotThrow(() => collection.DoFirst(m => m.Value = "yes"));
		}

		[Fact]
		public void When_running_on_a_single_item_collection()
		{
			var collection = new[] { new Model() };

			collection.DoFirst(m => m.Value = "yes");

			collection[0].Value.ShouldBe("yes");
		}

		[Fact]
		public void When_running_on_a_multi_item_collection()
		{
			var collection = new[] { new Model(), new Model() };

			collection.DoFirst(m => m.Value = "yes");

			collection[0].Value.ShouldBe("yes");
			collection[1].Value.ShouldBe("");
		}

		private class Model
		{
			public string Value = "";
		}
	}
}
