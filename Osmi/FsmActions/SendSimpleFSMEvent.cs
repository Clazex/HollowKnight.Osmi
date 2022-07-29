namespace Osmi.FsmActions;

[PublicAPI]
public class SendSimpleFSMEvent : FsmStateAction {
	public SimpleFSM.SimpleFSM? fsm;
	public string? eventName;

	public SendSimpleFSMEvent() {
	}

	public SendSimpleFSMEvent(SimpleFSM.SimpleFSM fsm, string eventName) {
		this.fsm = fsm;
		this.eventName = eventName;
	}

	public override void OnEnter() {
		if (eventName != null) {
			fsm?.SendEvent(eventName);
		}

		Finish();
	}
}
