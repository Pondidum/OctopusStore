using System.Collections.Generic;
using System.Linq;
using OctopusStore.Infrastructure;
using Shouldly;
using Xunit;

namespace OctopusStore.Tests.Infrastructure.ExtensionsTests
{
	public class ForEachTests
	{
		[Fact]
		public void When_run_on_an_empty_collection()
		{
			var iterated = 0;

			Enumerable.Empty<int>().ForEach(i => iterated++);

			iterated.ShouldBe(0);
		}

		[Fact]
		public void When_run_on_a_single_item_collection()
		{
			var iterated = 0;

			Enumerable.Range(0, 1).ForEach(i => iterated++);

			iterated.ShouldBe(1);
		}

		[Fact]
		public void When_run_on_a_multi_item_collection()
		{
			var iterated = 0;

			Enumerable.Range(0, 5).ForEach(i => iterated++);

			iterated.ShouldBe(5);
		}
	}
}
