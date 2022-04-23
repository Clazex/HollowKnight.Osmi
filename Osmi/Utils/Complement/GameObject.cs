using UGameObject = UnityEngine.GameObject;

namespace Osmi.Utils.Complement;

[PublicAPI]
public static class GameObject {
	public static UGameObject New(bool dontDestroyOnLoad = false) {
		UGameObject go = new();

		if (dontDestroyOnLoad) {
			UnityEngine.Object.DontDestroyOnLoad(go);
		}

		return go;
	}

	public static UGameObject New(string name, bool dontDestroyOnLoad = false) =>
		New(name, null!, dontDestroyOnLoad);

	public static UGameObject New(string name, Transform parent, bool dontDestroyOnLoad = false) {
		UGameObject go = new(name);
		go.SetParent(parent);

		if (dontDestroyOnLoad) {
			UnityEngine.Object.DontDestroyOnLoad(go);
		}

		return go;
	}

	public static UGameObject New(string name, Transform parent, params Type[] components) {
		UGameObject go = new(name, components);
		go.SetParent(parent);
		return go;
	}

	public static UGameObject New(string name, Transform parent, bool dontDestroyOnLoad, params Type[] components) {
		UGameObject go = new(name, components);
		go.SetParent(parent);

		if (dontDestroyOnLoad) {
			UnityEngine.Object.DontDestroyOnLoad(go);
		}

		return go;
	}
}
