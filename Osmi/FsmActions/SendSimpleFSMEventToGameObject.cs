using Osmi.SimpleFSM;

namespace Osmi.FsmActions;

[PublicAPI]
public class SendSimpleFSMEventToGameObject : FsmStateAction {
	public GameObject? gameObject;
	public string? eventName;

	public SendSimpleFSMEventToGameObject() {
	}

	public SendSimpleFSMEventToGameObject(string eventName) =>
		this.eventName = eventName;

	public SendSimpleFSMEventToGameObject(GameObject gameObject, string eventName) {
		this.gameObject = gameObject;
		this.eventName = eventName;
	}

	public override void OnEnter() {
		if (eventName != null) {
			(gameObject ?? Owner.gameObject).SendSimpleFSMEvent(eventName);
		}

		Finish();
	}
}
