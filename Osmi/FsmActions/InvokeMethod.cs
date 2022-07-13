namespace Osmi.FsmActions;

[PublicAPI]
public class InvokeMethod : FsmStateAction {
	public Action? method;

	public InvokeMethod() {
	}

	public InvokeMethod(Action method) =>
		this.method = method;

	public InvokeMethod(Action<FsmStateAction> method) =>
		this.method = method.Bind(this);

	public override void OnEnter() {
		method?.Invoke();
		Finish();
	}
}
