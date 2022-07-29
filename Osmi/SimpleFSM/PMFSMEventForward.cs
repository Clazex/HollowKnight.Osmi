namespace Osmi.SimpleFSM;

internal static class PMFSMEventForward {
	internal static void ForwardPMFSMComponentBroadcastEvent(
		On.PlayMakerFSM.orig_BroadcastEvent_FsmEvent orig,
		FsmEvent fsmEvent
	) {
		orig(fsmEvent);

		SimpleFSM.BroadcastPMFSMEvent(fsmEvent);
	}

	internal static void ForwardPMFSMUtilSendToGameObject(
		On.FSMUtility.orig_SendEventToGameObject_GameObject_FsmEvent_bool orig,
		GameObject go,
		FsmEvent ev,
		bool isRecursive
	) {
		orig(go, ev, isRecursive);

		SimpleFSM.SendPMFSMEventToGameObject(go, ev);
	}

	internal static void ForwardPMFSMBroadcastEvent(
		On.HutongGames.PlayMaker.Fsm.orig_BroadcastEvent_FsmEvent_bool orig,
		Fsm self,
		FsmEvent fsmEvent,
		bool excludeSelf
	) {
		orig(self, fsmEvent, excludeSelf);

		SimpleFSM.BroadcastPMFSMEvent(fsmEvent);
	}

	internal static void ForwardPMFSMBroadcastEventToGameObject(
		On.HutongGames.PlayMaker.Fsm.orig_BroadcastEventToGameObject_GameObject_FsmEvent_FsmEventData_bool_bool orig,
		Fsm self,
		GameObject go,
		FsmEvent fsmEvent,
		FsmEventData eventData,
		bool sendToChildren,
		bool excludeSelf
	) {
		orig(self, go, fsmEvent, eventData, sendToChildren, excludeSelf);

		SimpleFSM.SendPMFSMEventToGameObject(go, fsmEvent);
	}

	internal static void ForwardPMFSMSendEventToFsmOnGameObject(
		On.HutongGames.PlayMaker.Fsm.orig_SendEventToFsmOnGameObject_GameObject_string_FsmEvent orig,
		Fsm self,
		GameObject gameObject,
		string fsmName,
		FsmEvent fsmEvent
	) {
		orig(self, gameObject, fsmName, fsmEvent);

		SimpleFSM.SendPMFSMEventToGameObject(gameObject, fsmName, fsmEvent);
	}
}
