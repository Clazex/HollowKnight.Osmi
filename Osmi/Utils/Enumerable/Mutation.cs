namespace Osmi.Utils;

public static partial class EnumerableUtil {
	public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> self) =>
		self.FlatMap(Funcs.Identity);

	public static IEnumerable<T> Repeat<T>(this IEnumerable<T> self, int times) {
		for (int i = 0; i < times; i++) {
			foreach (T t in self) {
				yield return t;
			}
		}
	}


	public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> self, int index) {
		(IEnumerable<T> former, _, IEnumerable<T> latter) = self.SplitAt(index);
		return former.Concat(latter);
	}

	public static IEnumerable<T> Insert<T>(this IEnumerable<T> self, int index, T value) {
		if (index < 0 || index >= self.Count()) {
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		(IEnumerable<T> former, IEnumerable<T> latter) = self.Split(index);
		return former.Append(value).Concat(latter);
	}


	public static IEnumerable<T> Order<T, U>(this IEnumerable<T> self) =>
		self.OrderBy(Funcs.Identity);

	public static IEnumerable<T> OrderDescending<T, U>(this IEnumerable<T> self) =>
		self.OrderByDescending(Funcs.Identity);
}
