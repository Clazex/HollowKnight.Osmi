namespace Osmi.Utils;

[PublicAPI]
public static class GameObjectUtil {
	public static void SetParent(this GameObject self, Transform parent) =>
		self.transform.parent = parent;

	public static void SetParent(this GameObject self, GameObject parent) =>
		self.SetParent(parent.transform);

	public static void SetRoot(this GameObject self) =>
		self.SetParent((null as Transform)!);


	public static GameObject? Child(this GameObject self, string name) =>
		self.transform.Find(name)?.gameObject;

	public static GameObject? Child(this GameObject self, params string[] path) =>
		self.transform.Find(path.Join('/'))?.gameObject;

	public static IEnumerable<GameObject> GetChildren(this GameObject self) {
		foreach (Transform child in self.transform) {
			yield return child.gameObject;
		}
	}
}
