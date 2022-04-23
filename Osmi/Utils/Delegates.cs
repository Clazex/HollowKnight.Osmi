namespace Osmi.Utils;

[PublicAPI]
public static class Delegates {
	public static Func<T, T> Identity<T>() => Funcs.Identity;

	public static Action<T> Drop<T>() => Funcs.Drop;

	public static Action NoOp() => Funcs.NoOp;
	public static Action<A> NoOp<A>() => (A a) => { };
	public static Action<A, B> NoOp<A, B>() => (A a, B b) => { };
	public static Action<A, B, C> NoOp<A, B, C>() => (A a, B b, C c) => { };

	public static Func<A, bool> Pass<A>() => (A a) => true;
	public static Func<A, B, bool> Pass<A, B>() => (A a, B b) => true;
	public static Func<A, B, C, bool> Pass<A, B, C>() => (A a, B b, C c) => true;

	public static Func<A, bool> Block<A>() => (A a) => false;
	public static Func<A, B, bool> Block<A, B>() => (A a, B b) => false;
	public static Func<A, B, C, bool> Block<A, B, C>() => (A a, B b, C c) => false;

	public static Func<T?> Default<T>() => Funcs.Default<T>;

	public static Func<T> New<T>() where T : new() => Funcs.New<T>;

	public static Func<T> Constant<T>(T val) => Funcs.Constant(val);
	public static Func<U> Constant<T, U>(T val) where T : U => Funcs.Constant<T, U>(val);
}
