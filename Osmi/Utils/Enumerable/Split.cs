namespace Osmi.Utils;

public static partial class EnumerableUtil {
	public static IEnumerable<T> Slice<T>(this IEnumerable<T> self, int start) =>
		self.Slice(start, self.Count());

	public static IEnumerable<T> Slice<T>(this IEnumerable<T> self, int start, int end) {
		if (start < 0) {
			throw new ArgumentOutOfRangeException(nameof(start));
		}

		if (start > end || end > self.Count()) {
			throw new ArgumentOutOfRangeException(nameof(end));
		}

		int index = 0;
		int length = end - start;
		using IEnumerator<T> enumerator = self.Skip(start).GetEnumerator();

		while (index++ < length) {
			enumerator.MoveNext();
			yield return enumerator.Current;
		}
	}


	public static (IEnumerable<T> former, IEnumerable<T> latter) Split<T>(
		this IEnumerable<T> self, int index
	) {
		if (index == 0 && self.Empty()) {
			return (Of<T>(), Of<T>());
		}

		(IEnumerable<T> former, T value, IEnumerable<T> latter) = self.SplitAt(index);
		return (former, latter.Prepend(value));
	}

	public static (IEnumerable<T> former, T value, IEnumerable<T> latter) SplitAt<T>(
		this IEnumerable<T> self, int index
	) {
		if (index < 0 || index > self.Count()) {
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		if (self.Empty()) {
			throw new ArgumentException("Enumerable is empty");
		}

		List<T> former = new();
		int i = 0;
		using IEnumerator<T> enumerator = self.GetEnumerator();

		while (i++ < index) {
			enumerator.MoveNext();
			former.Add(enumerator.Current);
		}

		enumerator.MoveNext();
		T value = enumerator.Current!;

		return (former, value, enumerator.AsEnumerable());
	}
}
