namespace Osmi.FsmActions;

[PublicAPI]
public class BroadcastSimpleFSMEvent : FsmStateAction {
	public string? eventName;

	public BroadcastSimpleFSMEvent() {
	}

	public BroadcastSimpleFSMEvent(string eventName) =>
		this.eventName = eventName;

	public override void OnEnter() {
		if (eventName != null) {
			SimpleFSM.SimpleFSM.BroadcastEvent(eventName);
		}

		Finish();
	}
}
