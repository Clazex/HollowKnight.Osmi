namespace Osmi.FsmActions;

[PublicAPI]
public class WaitUntil : FsmStateAction {
	public Func<bool>? predicate;
	public FsmEvent? @event;

	public WaitUntil() {
	}

	public WaitUntil(Func<bool> predicate, FsmEvent @event) {
		this.predicate = predicate;
		this.@event = @event;
	}

	public override void OnEnter() {
		if (predicate == null) {
			Finish();
		}
	}

	public override void OnUpdate() {
		if (predicate != null && predicate()) {
			Fsm.Event(@event);
			Finish();
		}
	}
}
