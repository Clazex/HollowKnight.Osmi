using MonoMod.Utils;

using Osmi.Game;

namespace Osmi.SimpleFSM;

[PublicAPI]
public abstract partial class SimpleFSM {
	public Type Type { get; private set; } = null!;
	public string Name { get; private set; } = null!;

	public SimpleFSMInfo Info => info.Value;

	public SimpleFSMState State { get; private set; } = SimpleFSMState.Disabled;

	public string ActiveStateName { get; private set; } = "";

	public IReadOnlyDictionary<string, string> CurrentTransitions { get; private set; }
		= new Dictionary<string, string>();

	private Lazy<SimpleFSMInfo> info = null!;

	private IEnumerator? currentState = null;


	public void SendEvent(string eventName) {
		if (Info.GlobalTransitions.TryGetValue(eventName, out string stateName)) {
			SetState(stateName);
		} else if (CurrentTransitions.TryGetValue(eventName, out stateName)) {
			SetState(stateName);
		}
	}

	public void SendPMFSMEvent(FsmEvent fsmEvent) {
		if (AcceptPMFSMEvents) {
			SendEvent(fsmEvent.Name);
		}
	}

	public void SendEventDelayed(string eventName, float seconds) =>
		SendEventDelayed(eventName, new WaitForSeconds(seconds));

	public void SendEventDelayed(string eventName, YieldInstruction inst) =>
		GlobalCoroutineExecutor.Start(SendEventDelayedCoroutine(eventName, inst));

	public void SendEventDelayed(string eventName, CustomYieldInstruction inst) =>
		GlobalCoroutineExecutor.Start(SendEventDelayedCoroutine(eventName, inst));

	private IEnumerator SendEventDelayedCoroutine(string eventName, object inst) {
		yield return inst;
		SendEvent(eventName);
	}


	public void SetState(string stateName, params object[] args) {
		if (!Info.States.TryGetValue(stateName, out FastReflectionDelegate stateMethod)) {
			throw new ArgumentException($"Invalid state name {stateName}");
		}

		if (State == SimpleFSMState.Running || State == SimpleFSMState.Waiting) {
			State = SimpleFSMState.Suspended;
		}

		if (waitCoro != null) {
			GlobalCoroutineExecutor.Stop(waitCoro);
			waitCoro = null;
		}

		currentState = stateMethod.Invoke(this, args) as IEnumerator;
		CurrentTransitions = Info.Transitions[stateName];
		ActiveStateName = stateName;

		if (State == SimpleFSMState.Suspended) {
			State = SimpleFSMState.Running;
		}
	}


	#region Waiting

	private Coroutine? waitCoro = null;

	private void WaitFor(object inst) {
		State = SimpleFSMState.Waiting;
		waitCoro = GlobalCoroutineExecutor.Start(WaitCoroutine(inst));
	}

	private IEnumerator WaitCoroutine(object inst) {
		yield return inst;

		if (State == SimpleFSMState.Waiting) {
			State = SimpleFSMState.Running;
		}

		waitCoro = null;
	}

	#endregion
}
