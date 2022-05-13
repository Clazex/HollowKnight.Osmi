namespace Osmi.FsmActions;

[PublicAPI]
public class CheckIfInMapZone : FsmStateAction {
	public MapZone zone = MapZone.NONE;
	public FsmEvent? trueEvent;
	public FsmEvent? falseEvent;

	public override void OnEnter() {
		Fsm.Event(GameManager.instance?.sm.mapZone == zone ? trueEvent : falseEvent);
		Finish();
	}
}
