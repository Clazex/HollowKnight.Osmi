namespace Osmi.FsmActions;

[PublicAPI]
public class InvokePredicate : FsmStateAction {
	public Func<bool>? predicate;
	public FsmEvent? trueEvent;
	public FsmEvent? falseEvent;

	public InvokePredicate() {
	}

	public InvokePredicate(Func<bool> predicate) =>
		this.predicate = predicate;

	public InvokePredicate(Func<FsmStateAction, bool> predicate) =>
		this.predicate = predicate.Bind(this);

	public override void OnEnter() {
		if (predicate != null) {
			Fsm.Event(predicate?.Invoke() ?? false ? trueEvent : falseEvent);
		}

		Finish();
	}
}
