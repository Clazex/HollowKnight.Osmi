namespace Osmi.SimpleFSM;

public abstract partial class SimpleFSM {
	public const string FINISHED = "FINISHED";

	private static readonly List<SimpleFSM> existingFSMs = new();
	private static readonly List<SimpleFSM> activeFSMs = new();

	public static void Precompile<T>() where T : SimpleFSM =>
		_ = SimpleFSMInfo.map[typeof(T)];

	public static void Precompile(Type t) => _ = SimpleFSMInfo.map[t];

	public static void BroadcastEvent(string eventName, bool includeInactive = false) {
		foreach (SimpleFSM fsm in includeInactive ? existingFSMs : activeFSMs) {
			fsm.SendEvent(eventName);
		}
	}

	public static void BroadcastPMFSMEvent(FsmEvent fsmEvent, bool includeInactive = false) {
		foreach (SimpleFSM fsm in includeInactive ? existingFSMs : activeFSMs) {
			fsm.SendPMFSMEvent(fsmEvent);
		}
	}

	public static void SendEventToGameObject(GameObject go, string eventName, bool includeInactive = false) {
		foreach (SimpleFSM fsm in includeInactive ? existingFSMs : activeFSMs) {
			if (fsm.gameObject == go) {
				fsm.SendEvent(eventName);
			}
		}
	}

	public static void SendPMFSMEventToGameObject(GameObject go, FsmEvent fsmEvent, bool includeInactive = false) {
		foreach (SimpleFSM fsm in includeInactive ? existingFSMs : activeFSMs) {
			if (fsm.gameObject == go) {
				fsm.SendPMFSMEvent(fsmEvent);
			}
		}
	}

	public static void SendEventToGameObject(GameObject go, string fsmName, string eventName, bool includeInactive = false) {
		foreach (SimpleFSM fsm in includeInactive ? existingFSMs : activeFSMs) {
			if (fsm.gameObject == go && fsm.Name == fsmName) {
				fsm.SendEvent(eventName);
			}
		}
	}

	public static void SendPMFSMEventToGameObject(GameObject go, string fsmName, FsmEvent fsmEvent, bool includeInactive = false) {
		foreach (SimpleFSM fsm in includeInactive ? existingFSMs : activeFSMs) {
			if (fsm.gameObject == go && fsm.Name == fsmName) {
				fsm.SendPMFSMEvent(fsmEvent);
			}
		}
	}


	static SimpleFSM() {
		On.PlayMakerFSM.BroadcastEvent_FsmEvent += PMFSMEventForward.ForwardPMFSMComponentBroadcastEvent;
		On.FSMUtility.SendEventToGameObject_GameObject_FsmEvent_bool += PMFSMEventForward.ForwardPMFSMUtilSendToGameObject;
		On.HutongGames.PlayMaker.Fsm.BroadcastEvent_FsmEvent_bool += PMFSMEventForward.ForwardPMFSMBroadcastEvent;
		On.HutongGames.PlayMaker.Fsm.BroadcastEventToGameObject_GameObject_FsmEvent_FsmEventData_bool_bool += PMFSMEventForward.ForwardPMFSMBroadcastEventToGameObject;
		On.HutongGames.PlayMaker.Fsm.SendEventToFsmOnGameObject_GameObject_string_FsmEvent += PMFSMEventForward.ForwardPMFSMSendEventToFsmOnGameObject;
	}
}
