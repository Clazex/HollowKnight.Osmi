namespace Osmi.Utils;

[PublicAPI]
public static class GameObjectUtil {
	private static readonly GameObject holder = New("Osmi MonoBehaviour Holder", true);

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


	public static TransformDelegate GetDelegate(this Transform self) => new(self);

	public static TransformDelegate GetTransformDelegate(this GameObject self) =>
		self.transform.GetDelegate();


	public static GameObject New(bool dontDestroyOnLoad = false) {
		GameObject go = new();

		if (dontDestroyOnLoad) {
			UObject.DontDestroyOnLoad(go);
		}

		return go;
	}

	public static GameObject New(string name, bool dontDestroyOnLoad = false) =>
		New(name, null!, dontDestroyOnLoad);

	public static GameObject New(string name, Transform parent, bool dontDestroyOnLoad = false) {
		GameObject go = new(name);
		go.SetParent(parent);

		if (dontDestroyOnLoad) {
			UObject.DontDestroyOnLoad(go);
		}

		return go;
	}

	public static GameObject New(string name, Transform parent, params Type[] components) {
		GameObject go = new(name, components);
		go.SetParent(parent);
		return go;
	}

	public static GameObject New(string name, Transform parent, bool dontDestroyOnLoad, params Type[] components) {
		GameObject go = new(name, components);
		go.SetParent(parent);

		if (dontDestroyOnLoad) {
			UnityEngine.Object.DontDestroyOnLoad(go);
		}

		return go;
	}


	public static T Instantiate<T>(T original, Vector3 position)
		where T : UObject =>
		UObject.Instantiate(original, position, Quaternion.identity);

	public static T Instantiate<T>(T original, Vector3 position, Transform parent)
		where T : UObject =>
		UObject.Instantiate(original, position, Quaternion.identity, parent);


	public static T CreateHolder<T>(string name = "") where T : MonoBehaviour =>
		New(string.IsNullOrWhiteSpace(name) ? typeof(T).Name + " Holder" : name, holder.transform).AddComponent<T>();
}
