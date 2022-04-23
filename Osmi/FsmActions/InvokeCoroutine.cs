namespace Osmi.FsmActions;

[PublicAPI]
public class InvokeCoroutine : FsmStateAction {
	public Func<IEnumerator> coroutine;
	public bool sync;

	public InvokeCoroutine(Func<IEnumerator> coroutine, bool sync = false) {
		this.coroutine = coroutine;
		this.sync = sync;
	}

	public InvokeCoroutine(IEnumerator coroutine, bool sync = false) {
		this.coroutine = Funcs.Constant(coroutine);
		this.sync = sync;
	}

	public override void OnEnter() {
		if (sync) {
			Fsm.Owner.StartCoroutine(SyncCoroutine());
		} else {
			Fsm.Owner.StartCoroutine(coroutine.Invoke());
			Finish();
		}
	}

	private IEnumerator SyncCoroutine() {
		yield return coroutine.Invoke();
		Finish();
	}
}
