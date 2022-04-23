namespace Osmi.Utils.Tap;

[PublicAPI]
public static class TapUtil {
	public static T Tap<T>(this T self, Action<T> f) {
		f(self);
		return self;
	}

	public static U Thru<T, U>(this T self, Func<T, U> f) =>
		f(self);


	public static Func<T, Action<T>, T> TapFunc<T>() =>
		(T t, Action<T> f) => t.Tap(f);

	public static Func<Action<T>, T> TapFunc<T>(this T self) =>
		TapFunc<T>().Bind1(self);

	public static Func<T, T> TapFunc<T>(Action<T> f) =>
		TapFunc<T>().Bind2(f);
}
