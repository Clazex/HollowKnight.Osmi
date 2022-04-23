namespace Osmi.FsmActions;

[PublicAPI]
public class CheckSceneName : FsmStateAction {
	public string sceneName = "";
	public FsmEvent? trueEvent;
	public FsmEvent? falseEvent;

	public CheckSceneName(string sceneName) =>
		this.sceneName = sceneName;

	public override void OnEnter() =>
		Fsm.Event(GameManager.instance?.sceneName == sceneName ? trueEvent : falseEvent);
}
