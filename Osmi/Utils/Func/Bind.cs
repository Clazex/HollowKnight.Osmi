namespace Osmi.Utils;

public static partial class FuncUtil {
	public static Func<T> Bind<A, T>(this Func<A, T> self, A a) =>
		() => self(a);

	public static Func<B, T> Bind1<A, B, T>(this Func<A, B, T> self, A a) =>
		(B b) => self(a, b);
	public static Func<A, T> Bind2<A, B, T>(this Func<A, B, T> self, B b) =>
		(A a) => self(a, b);

	public static Func<B, C, T> Bind1<A, B, C, T>(this Func<A, B, C, T> self, A a) =>
		(B b, C c) => self(a, b, c);
	public static Func<A, C, T> Bind2<A, B, C, T>(this Func<A, B, C, T> self, B b) =>
		(A a, C c) => self(a, b, c);
	public static Func<A, B, T> Bind3<A, B, C, T>(this Func<A, B, C, T> self, C c) =>
		(A a, B b) => self(a, b, c);

	public static Func<B, C, D, T> Bind1<A, B, C, D, T>(this Func<A, B, C, D, T> self, A a) =>
		(B b, C c, D d) => self(a, b, c, d);
	public static Func<A, C, D, T> Bind2<A, B, C, D, T>(this Func<A, B, C, D, T> self, B b) =>
		(A a, C c, D d) => self(a, b, c, d);
	public static Func<A, B, D, T> Bind3<A, B, C, D, T>(this Func<A, B, C, D, T> self, C c) =>
		(A a, B b, D d) => self(a, b, c, d);
	public static Func<A, B, C, T> Bind4<A, B, C, D, T>(this Func<A, B, C, D, T> self, D d) =>
		(A a, B b, C c) => self(a, b, c, d);


	public static Action Bind<A>(this Action<A> self, A a) =>
		() => self(a);

	public static Action<B> Bind1<A, B>(this Action<A, B> self, A a) =>
		(B b) => self(a, b);
	public static Action<A> Bind2<A, B>(this Action<A, B> self, B b) =>
		(A a) => self(a, b);

	public static Action<B, C> Bind1<A, B, C>(this Action<A, B, C> self, A a) =>
		(B b, C c) => self(a, b, c);
	public static Action<A, C> Bind2<A, B, C>(this Action<A, B, C> self, B b) =>
		(A a, C c) => self(a, b, c);
	public static Action<A, B> Bind3<A, B, C>(this Action<A, B, C> self, C c) =>
		(A a, B b) => self(a, b, c);

	public static Action<B, C, D> Bind1<A, B, C, D, T>(this Action<A, B, C, D> self, A a) =>
		(B b, C c, D d) => self(a, b, c, d);
	public static Action<A, C, D> Bind2<A, B, C, D, T>(this Action<A, B, C, D> self, B b) =>
		(A a, C c, D d) => self(a, b, c, d);
	public static Action<A, B, D> Bind3<A, B, C, D, T>(this Action<A, B, C, D> self, C c) =>
		(A a, B b, D d) => self(a, b, c, d);
	public static Action<A, B, C> Bind4<A, B, C, D, T>(this Action<A, B, C, D> self, D d) =>
		(A a, B b, C c) => self(a, b, c, d);
}
