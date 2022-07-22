namespace Osmi.Game;

public sealed class InSceneCoroutineExecutor : MonoBehaviour {
	private static readonly InSceneCoroutineExecutor instance
		= GameObjectUtil.CreateHolder<InSceneCoroutineExecutor>("In Scene Coroutine Executor");

	public static Coroutine Start(IEnumerator enumerator) =>
		instance.StartCoroutine(enumerator);

	public static void Stop(IEnumerator enumerator) =>
		instance.StopCoroutine(enumerator);

	public static void Stop(Coroutine coroutine) =>
		instance.StopCoroutine(coroutine);

	internal static void StopAll() =>
		instance.StopAllCoroutines();

	public static Coroutine SetTimeOut(float timeOut, Action action) => instance.SetTimeOut(timeOut, action);

	public static Coroutine SetImmediate(Action action) => instance.SetImmediate(action);


	static InSceneCoroutineExecutor() =>
		OsmiHooks.SceneChangeHook += (_, _) => StopAll();
}
