namespace Osmi.ChildRefs;

[PublicAPI]
public static class ChildRefsExtension {
	public static void InitChildRefs(this Component self) =>
		ChildRef.map[self.GetType()].Invoke(self);
}
