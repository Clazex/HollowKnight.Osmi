namespace Osmi.Utils;

public static partial class FuncUtil {
	public static Func<bool> Negate(this Func<bool> self) => () => !self.Invoke();

	public static Func<A, bool> Negate<A>(this Func<A, bool> self) =>
		(A a) => !self.Invoke(a);

	public static Func<A, B, bool> Negate<A, B>(this Func<A, B, bool> self) =>
		(A a, B b) => !self.Invoke(a, b);

	public static Func<A, B, C, bool> Negate<A, B, C>(this Func<A, B, C, bool> self) =>
		(A a, B b, C c) => !self.Invoke(a, b, c);

	public static Func<A, B, C, D, bool> Negate<A, B, C, D>(this Func<A, B, C, D, bool> self) =>
		(A a, B b, C c, D d) => !self.Invoke(a, b, c, d);

	public static Func<A, B, C, D, E, bool> Negate<A, B, C, D, E>(this Func<A, B, C, D, E, bool> self) =>
		(A a, B b, C c, D d, E e) => !self.Invoke(a, b, c, d, e);

	public static Func<A, B, C, D, E, G, bool> Negate<A, B, C, D, E, G>(this Func<A, B, C, D, E, G, bool> self) =>
		(A a, B b, C c, D d, E e, G g) => !self.Invoke(a, b, c, d, e, g);
}
