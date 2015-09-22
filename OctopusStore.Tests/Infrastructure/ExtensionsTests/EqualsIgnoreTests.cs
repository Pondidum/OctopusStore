using OctopusStore.Infrastructure;
using Shouldly;
using Xunit;

namespace OctopusStore.Tests.Infrastructure.ExtensionsTests
{
	public class EqualsIgnoreTests
	{
		[Fact]
		public void When_the_strings_match_exactly()
		{
			"test".EqualsIgnore("test").ShouldBe(true);
		}

		[Fact]
		public void When_the_string_differ_by_case()
		{
			"test".EqualsIgnore("tESt").ShouldBe(true);
		}

		[Fact]
		public void WHen_the_strings_are_different()
		{
			"test".EqualsIgnore("no.").ShouldBe(false);
		}
	}
}
