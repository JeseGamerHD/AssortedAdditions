using System;
using System.Collections.Generic;

namespace AssortedAdditions.Helpers
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Shuffles the elements in the list into a random order
		/// </summary>
		public static void Shuffle<T>(this IList<T> list)
		{
			Random random = new();
			int n = list.Count;
			int k;

			while (n > 1)
			{
				n--;
				k = random.Next(n + 1);
				(list[n], list[k]) = (list[k], list[n]);
			}
		}
	}
}
