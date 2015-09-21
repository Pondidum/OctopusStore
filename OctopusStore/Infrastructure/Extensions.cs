﻿using System;
using System.Collections.Generic;
using System.Text;

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

		public static string ToBase64(this string self)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(self));
		}
	}
}
