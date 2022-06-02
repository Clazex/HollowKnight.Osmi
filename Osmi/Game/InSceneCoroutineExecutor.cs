namespace Osmi.Game;

public sealed class InSceneCoroutineExecutor : MonoBehaviour {
	private static readonly InSceneCoroutineExecutor instance
		= GameObjectUtil.CreateHolder<InSceneCoroutineExecutor>();

	public static Coroutine Start(IEnumerator enumerator) =>
		instance.StartCoroutine(enumerator);

	public static void Stop(IEnumerator enumerator) =>
		instance.StopCoroutine(enumerator);

	public static void Stop(Coroutine coroutine) =>
		instance.StopCoroutine(coroutine);

	public static void StopAll() =>
		instance.StopAllCoroutines();

	static InSceneCoroutineExecutor() =>
		OsmiHooks.SceneChangeHook += (_, _) => StopAll();
}
