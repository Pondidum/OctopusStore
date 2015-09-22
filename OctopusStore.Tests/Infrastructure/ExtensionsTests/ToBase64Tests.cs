using OctopusStore.Infrastructure;
using Shouldly;
using Xunit;

namespace OctopusStore.Tests.Infrastructure.ExtensionsTests
{
	public class ToBase64Tests
	{
		[Fact]
		public void When_converting_a_string()
		{
			"test".ToBase64().ShouldBe("dGVzdA==");
		}
	}
}
