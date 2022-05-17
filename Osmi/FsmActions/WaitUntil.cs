namespace Osmi.FsmActions;

[PublicAPI]
public class WaitUntil : FsmStateAction {
	public Func<bool> predicate;
	public FsmEvent @event;

	internal WaitUntil(Func<bool> predicate, FsmEvent @event) {
		this.predicate = predicate;
		this.@event = @event;
	}

	public override void OnUpdate() {
		if (predicate()) {
			Fsm.Event(@event);
			Finish();
		}
	}
}
