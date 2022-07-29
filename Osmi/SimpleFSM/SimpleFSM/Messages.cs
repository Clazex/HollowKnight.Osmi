using Osmi.ChildRefs;
using Osmi.Game;

namespace Osmi.SimpleFSM;

public abstract partial class SimpleFSM : MonoBehaviour {
	protected void Awake() {
		Type = GetType();
		Name = Type.Name;
		info = new(() => SimpleFSMInfo.map[Type]);
		existingFSMs.Add(this);
	}

	protected void Start() {
		this.InitChildRefs();
		SetState(nameof(Init));
	}

	protected void OnEnable() {
		if (RestartOnEnable) {
			SetState(nameof(Init));
		}

		activeFSMs.Add(this);
		State = SimpleFSMState.Running;
	}

	protected void OnDisable() {
		State = SimpleFSMState.Disabled;

		if (waitCoro != null) {
			GlobalCoroutineExecutor.Stop(waitCoro);
			waitCoro = null;
		}

		_ = activeFSMs.Remove(this);
	}

	protected void OnDestroy() =>
		existingFSMs.Remove(this);

	protected void Update() {
		if (State != SimpleFSMState.Running) {
			return;
		}

		if (currentState == null || !currentState.MoveNext()) {
			State = SimpleFSMState.Suspended;
			currentState = null;
			SendEvent(FINISHED);
			return;
		}

		object current = currentState.Current;
		if (current is string eventName) {
			SendEvent(eventName);
		} else if (current is YieldInstruction inst) {
			WaitFor(inst);
		} else if (current is CustomYieldInstruction customInst) {
			WaitFor(customInst);
		} else if (current is (string delayedEventName, YieldInstruction delayInst)) {
			SendEventDelayed(delayedEventName, delayInst);
		} else if (current is (string customDelayedEventName, CustomYieldInstruction customDelayInst)) {
			SendEventDelayed(customDelayedEventName, customDelayInst);
		}
	}
}
