using System.Threading.Tasks;

namespace Osmi.Utils;

[PublicAPI]
public static partial class EnumerableUtil {
	public static IEnumerable<T> Evaluate<T>(this IEnumerable<T> self) =>
		self.ToArray().AsEnumerable();

	public static IEnumerable<T> Of<T>(params T[] values) => values;

	public static IEnumerable<T> Of<T>(T val, int count) =>
		Enumerable.Repeat(val, count);

	public static void ParallelForEach<T>(this IEnumerable<T> self, Action<T> f) =>
		Parallel.ForEach(self, f);

	public static bool TryAdd<K, V>(this IDictionary<K, V> self, K key, V val) {
		if (self.ContainsKey(key)) {
			return false;
		}

		self[key] = val;
		return true;
	}
}
