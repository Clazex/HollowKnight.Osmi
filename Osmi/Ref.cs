namespace Osmi;

[PublicAPI]
public static class Ref {
	public static GameManager GM => GameManager.instance;

	public static HeroController HC => HeroController.instance;

	public static PlayerData PD => PlayerData.instance;

	public static SceneData SD => SceneData.instance;

	public static GameCameras GC => GameCameras.instance;
}
