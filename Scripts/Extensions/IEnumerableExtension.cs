using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalDungeon.Extensions;

public static class IEnumerableExtension {
	public static readonly Random Random = new();

	public static T GetRandomElement<T>(this IEnumerable<T> collection) {
		var list = collection.ToArray();
		var index = Random.Next(list.Length);

		return list[index];
	}
}