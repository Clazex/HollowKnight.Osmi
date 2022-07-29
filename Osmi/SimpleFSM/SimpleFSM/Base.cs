namespace Osmi.SimpleFSM;

public abstract partial class SimpleFSM {
	protected virtual bool AcceptPMFSMEvents => false;
	protected virtual bool RestartOnEnable => false;

	[FSMState]
	protected virtual IEnumerator Init() {
		yield break;
	}
}
