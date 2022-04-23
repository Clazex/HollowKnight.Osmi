using Osmi.Utils.Tap;

namespace Osmi.Utils;

public static partial class EnumerableUtil {
	public static void ForEach<T>(this IEnumerable<T> self, Action<T> f) {
		foreach (T i in self) {
			f(i);
		}
	}

	public static IEnumerable<U> Map<T, U>(this IEnumerable<T> self, Func<T, U> f) =>
		self.Select(f);

	public static IEnumerable<U> FlatMap<T, U>(this IEnumerable<T> self, Func<T, IEnumerable<U>> f) =>
		self.SelectMany(f);

	public static IEnumerable<T> Tap<T>(this IEnumerable<T> self, Action<T> f) =>
		self.Map(TapUtil.TapFunc(f));


	public static IEnumerable<T> Filter<T>(this IEnumerable<T> self, Func<T, bool> f) =>
		self.Where(f);

	public static IEnumerable<T> Reject<T>(this IEnumerable<T> self, Func<T, bool> f) =>
		self.Where(f.Negate());


	public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> self, Func<T, bool> f) =>
		self.TakeWhile(f.Negate());

	public static IEnumerable<T> SkipUntil<T>(this IEnumerable<T> self, Func<T, bool> f) =>
		self.SkipWhile(f.Negate());
}
