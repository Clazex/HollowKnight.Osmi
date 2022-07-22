namespace Osmi.Utils;

public static partial class FuncUtil {
	public static Coroutine SetTimeOut(this MonoBehaviour self, float timeOut, Action action) =>
		self.StartCoroutine(TimeOutCoroutine(action, timeOut));

	public static Coroutine SetImmediate(this MonoBehaviour self, Action action) =>
		self.SetTimeOut(0, action);

	private static IEnumerator TimeOutCoroutine(Action action, float timeOut) {
		yield return new WaitForSeconds(timeOut);
		action.Invoke();
	}
}
