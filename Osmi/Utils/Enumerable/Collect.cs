namespace Osmi.Utils;

public static partial class EnumerableUtil {
	public static T Fold<T>(this IEnumerable<T> self, Func<T, T, T> f) =>
		self.Aggregate(f);

	public static U Reduce<T, U>(this IEnumerable<T> self, Func<U, T, U> f, U init) {
		U acc = init;

		foreach (T i in self) {
			acc = f(acc, i);
		}

		return acc;
	}

	public static string Collect(this IEnumerable<char> self) =>
		new(self.ToArray());

	public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> self) =>
		self.ToDictionary(pair => pair.Key, pair => pair.Value);

	public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<(K key, V value)> self) =>
		self.ToDictionary(tuple => tuple.key, tuple => tuple.value);
}
