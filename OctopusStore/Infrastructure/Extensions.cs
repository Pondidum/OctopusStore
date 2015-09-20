using System;
using System.Collections.Generic;

namespace OctopusStore.Infrastructure
{
	public static class Extensions
	{
		public static bool EqualsIgnore(this string self, string value)
		{
			return self.Equals(value, StringComparison.OrdinalIgnoreCase);
		}

		public static void DoFirst<T>(this IEnumerable<T> self, Action<T> action)
		{
			using (var enumerator = self.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					action(enumerator.Current);
				}
			}
		}
	}
}
