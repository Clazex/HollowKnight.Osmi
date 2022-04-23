namespace Osmi.Utils;

public static partial class EnumerableUtil {
	public static bool Includes<T>(this IEnumerable<T> self, T val) =>
		self.Contains(val);

	public static bool Empty<T>(this IEnumerable<T> self) => !self.Any();

	public static bool Some<T>(this IEnumerable<T> self, Func<T, bool> f) =>
		self.Any(f);

	public static bool Every<T>(this IEnumerable<T> self, Func<T, bool> f) =>
		self.All(f);

	public static int FindIndex<T>(this IEnumerable<T> self, Func<T, bool> f) {
		int index = 0;

		foreach (T i in self) {
			if (f(i)) {
				return index;
			}

			index++;
		}

		return -1;
	}
}
