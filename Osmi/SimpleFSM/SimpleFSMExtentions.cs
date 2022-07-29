using Osmi.FsmActions;

namespace Osmi.SimpleFSM;

[PublicAPI]
public static class SimpleFSMExtensions {
	public const string PROXY_PMFSM_NAME = "SimpleFSM Event Proxy";

	public static IEnumerable<SimpleFSM> LocateSimpleFSMs(this GameObject self, string typeName) =>
		self.GetComponents<SimpleFSM>()
			.Filter(fsm => fsm.Type.Name == typeName);

	public static SimpleFSM? LocateSimpleFSM(this GameObject self, string typeName) =>
		self.LocateSimpleFSMs(typeName).FirstOrDefault();

	public static void SendSimpleFSMEvent(this GameObject self, string eventName) =>
		SimpleFSM.SendEventToGameObject(self, eventName);

	public static PlayMakerFSM CreatePlayMakerFSMEventProxy(this GameObject self, string fsmEvent) =>
		self.CreatePlayMakerFSMEventProxy(FsmEvent.GetFsmEvent(fsmEvent));

	public static PlayMakerFSM CreatePlayMakerFSMEventProxy(this GameObject self, FsmEvent fsmEvent) {
		string name = fsmEvent.Name;

		if (self.LocateMyFSM(PROXY_PMFSM_NAME) is not PlayMakerFSM pmfsm) {
			pmfsm = self.AddComponent<PlayMakerFSM>();
			pmfsm.FsmName = PROXY_PMFSM_NAME;
		}

		Fsm fsm = pmfsm.Fsm;

		if (fsm.GetState(name) == null) {
			FsmState[] states = fsm.States;
			Array.Resize(ref states, states.Length + 1);
			FsmState state = states[states.Length - 1] = new FsmState(fsm) {
				Name = name,
				Actions = new FsmStateAction[] {
					new SendSimpleFSMEventToGameObject(name)
				}
			};
			fsm.States = states;

			FsmTransition[] globalTransitions = fsm.GlobalTransitions;
			Array.Resize(ref globalTransitions, globalTransitions.Length + 1);
			globalTransitions[globalTransitions.Length - 1] = new FsmTransition() {
				FsmEvent = fsmEvent,
				ToFsmState = state,
				ToState = name
			};
			fsm.GlobalTransitions = globalTransitions;
		}

		pmfsm.enabled = true;
		return pmfsm;
	}
}
