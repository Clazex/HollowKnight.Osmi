namespace Osmi.FsmActions;

[PublicAPI]
public class EmitEvent : FsmStateAction {
	public FsmEvent @event;

	public EmitEvent(FsmEvent @event) =>
		this.@event = @event;

	public EmitEvent(string eventName)
		: this(FsmEvent.GetFsmEvent(eventName)) {
	}

	public override void OnEnter() {
		Fsm.Event(@event);
		Finish();
	}
}
