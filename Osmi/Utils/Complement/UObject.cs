
using Object = UnityEngine.Object;

namespace Osmi.Utils.Complement;

[PublicAPI]
public static class UObject {
	public static T Instantiate<T>(T original, Vector3 position)
		where T : Object =>
		Object.Instantiate(original, position, Quaternion.identity);

	public static T Instantiate<T>(T original, Vector3 position, Transform parent)
		where T : Object =>
		Object.Instantiate(original, position, Quaternion.identity, parent);
}
