namespace Osmi.Utils;

[PublicAPI]
public static class Funcs {
	public static T Identity<T>(T self) => self;

	public static void Drop<T>(T t) {
		if (t is IDisposable d) {
			d.Dispose();
		}
	}

	public static void NoOp() { }
	public static void NoOp(params object?[] _) { }

	public static bool Pass(params object?[] _) => true;
	public static bool Block(params object?[] _) => false;

	public static T? Default<T>() => default;

	public static T New<T>() where T : new() => new();

	public static Func<T> Constant<T>(T val) => () => val;
	public static Func<U> Constant<T, U>(T val) where T : U => () => val;
}
