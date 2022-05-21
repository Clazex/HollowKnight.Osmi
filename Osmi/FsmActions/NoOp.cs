namespace Osmi.FsmActions;

public sealed class NoOp : FsmStateAction {
	public override void OnEnter() => Finish();
}
