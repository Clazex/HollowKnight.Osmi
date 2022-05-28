namespace Osmi.Utils;

[PublicAPI]
public static class TupleUtil {
	public static (K key, V value) ToTuple<K, V>(this KeyValuePair<K, V> self) =>
		(self.Key, self.Value);

	public static (float x, float y, float z) ToTuple(this Vector3 self) =>
		(self.x, self.y, self.z);

	public static (float x, float y) ToTuple(this Vector2 self) =>
		(self.x, self.y);


	public static void Deconstruct<K, V>(this KeyValuePair<K, V> self, out K key, out V value) =>
		(key, value) = self.ToTuple();

	public static void Deconstruct(this Vector3 self, out float x, out float y, out float z) =>
		(x, y, z) = self.ToTuple();

	public static void Desconstruct(this Vector2 self, out float x, out float y) =>
		(x, y) = self.ToTuple();


	public static Vector2 AsVector2(this Vector3 self) => new(self.x, self.y);

	public static Vector2 GetUnitVector(this Vector2 self) => self / self.magnitude;

	public static Vector3 GetUnitVector(this Vector3 self) => self / self.magnitude;
}
