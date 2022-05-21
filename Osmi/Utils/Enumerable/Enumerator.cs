namespace Osmi.Utils;

public static partial class EnumerableUtil {
	public static IEnumerator<T> Move<T>(this IEnumerator<T> self, int step = 1) {
		while (step-- > 0) {
			_ = self.MoveNext();
		}

		return self;
	}

	public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> self) {
		while (self.MoveNext()) {
			yield return self.Current;
		}
	}
}
