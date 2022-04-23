namespace Osmi.Utils;

[PublicAPI]
public static partial class FuncUtil {
	public static Func<U> Thru<T, U>(this Func<T> self, Func<T, U> f) =>
		() => f(self());

	public static Func<A, U> Thru<A, T, U>(this Func<A, T> self, Func<T, U> f) =>
		(A a) => f(self(a));

	public static Func<A, B, U> Thru<A, B, T, U>(this Func<A, B, T> self, Func<T, U> f) =>
		(A a, B b) => f(self(a, b));

	public static Func<A, B, C, U> Thru<A, B, C, T, U>(this Func<A, B, C, T> self, Func<T, U> f) =>
		(A a, B b, C c) => f(self(a, b, c));

	public static Func<A, B, C, D, U> Thru<A, B, C, D, T, U>(this Func<A, B, C, D, T> self, Func<T, U> f) =>
		(A a, B b, C c, D d) => f(self(a, b, c, d));

	public static Func<A, B, C, D, E, U> Thru<A, B, C, D, E, T, U>(this Func<A, B, C, D, E, T> self, Func<T, U> f) =>
		(A a, B b, C c, D d, E e) => f(self(a, b, c, d, e));

	public static Func<A, B, C, D, E, G, U> Thru<A, B, C, D, E, G, T, U>(this Func<A, B, C, D, E, G, T> self, Func<T, U> f) =>
		(A a, B b, C c, D d, E e, G g) => f(self(a, b, c, d, e, g));



	public static Action Thru<T>(this Func<T> self, Action<T> f) =>
		() => f(self());

	public static Action<A> Thru<A, T, U>(this Func<A, T> self, Action<T> f) =>
		(A a) => f(self(a));

	public static Action<A, B> Thru<A, B, T, U>(this Func<A, B, T> self, Action<T> f) =>
		(A a, B b) => f(self(a, b));

	public static Action<A, B, C> Thru<A, B, C, T, U>(this Func<A, B, C, T> self, Action<T> f) =>
		(A a, B b, C c) => f(self(a, b, c));

	public static Action<A, B, C, D> Thru<A, B, C, D, T, U>(this Func<A, B, C, D, T> self, Action<T> f) =>
		(A a, B b, C c, D d) => f(self(a, b, c, d));

	public static Action<A, B, C, D, E> Thru<A, B, C, D, E, T, U>(this Func<A, B, C, D, E, T> self, Action<T> f) =>
		(A a, B b, C c, D d, E e) => f(self(a, b, c, d, e));

	public static Action<A, B, C, D, E, G> Thru<A, B, C, D, E, G, T, U>(this Func<A, B, C, D, E, G, T> self, Action<T> f) =>
		(A a, B b, C c, D d, E e, G g) => f(self(a, b, c, d, e, g));
}
