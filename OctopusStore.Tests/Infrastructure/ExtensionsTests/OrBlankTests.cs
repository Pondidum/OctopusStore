using System.Linq;
using OctopusStore.Infrastructure;
using Shouldly;
using Xunit;

namespace OctopusStore.Tests.Infrastructure.ExtensionsTests
{
	public class OrBlankTests
	{
		[Fact]
		public void When_run_on_an_empty_collection()
		{
			var items = Enumerable.Empty<int>().OrBlank(() => 5);

			items.ShouldBe(new[] { 5 });
		}

		[Fact]
		public void When_run_on_a_multi_item_collection()
		{
			var iterated = 0;
			var blankAdded = false;

			Enumerable
				.Range(0, 5)
				.OrBlank(() =>
				{
					blankAdded = true;
					return 10;
				})
				.ForEach(i => iterated++);

			iterated.ShouldBe(5);
			blankAdded.ShouldBe(false);
		}
	}
}
