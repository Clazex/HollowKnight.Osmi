using Mono.Cecil.Cil;

using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace Osmi;

[PublicAPI]
public static class OsmiHooks {
	/// <summary>
	/// Called when entering main menu for the first time <br />
	/// Hooking new methods after that will result in immediate invocation
	/// </summary>
	public static event Action GameInitializedHook {
		add {
			if (gameInitializedHookCalled) {
				value?.Invoke();
				return;
			}

			InternalGameInitializedHook += value;
		}
		remove => InternalGameInitializedHook -= value;
	}
	private static bool gameInitializedHookCalled = false;
	private static event Action InternalGameInitializedHook = null!;

	internal static IEnumerator CheckGameInitialized() {
		yield return new WaitUntil(() => GameObject.Find("LogoTitle") != null);

		gameInitializedHookCalled = true;
		if (InternalGameInitializedHook == null) {
			yield break;
		}

		foreach (Action a in InternalGameInitializedHook.GetInvocationList()) {
			try {
				a.Invoke();
			} catch (Exception e) {
				Logger.LogError(e.ToString());
			}
		}

		InternalGameInitializedHook = null!;
	}


	/// <summary>
	/// Equivalent to <see cref="UIManager.EditMenus"/>,
	/// but ensure to be called after MAPI finished building menus
	/// </summary>
	public static event Action MenuBuildHook = null!;

	private static void OnMenuBuild() {
		Logger.LogFine($"{nameof(OnMenuBuild)} Invoked");

		if (MenuBuildHook == null) {
			return;
		}

		foreach (Action a in MenuBuildHook.GetInvocationList()) {
			try {
				a.Invoke();
			} catch (Exception e) {
				Logger.LogError(e.ToString());
			}
		}
	}


	/// <summary>
	/// Called after entering a new or existing save,
	/// ensures to be after <see cref="On.HeroController.Start"/>
	/// </summary>
	public static event Action AfterEnterSaveHook = null!;

	private static void OnAfterEnterSave() {
		Ref.GM.OnFinishedEnteringScene -= OnAfterEnterSave;
		Logger.LogFine($"{nameof(OnAfterEnterSave)} Invoked");

		if (AfterEnterSaveHook == null) {
			return;
		}

		foreach (Action a in AfterEnterSaveHook.GetInvocationList()) {
			try {
				a.Invoke();
			} catch (Exception e) {
				Logger.LogError(e.ToString());
			}
		}
	}

	private static void HookOnAfterEnterSave(On.HeroController.orig_Start orig, HeroController self) {
		orig(self);
		Ref.GM.OnFinishedEnteringScene -= OnAfterEnterSave;
		Ref.GM.OnFinishedEnteringScene += OnAfterEnterSave;
	}


	/// <summary>
	///	Equivalent to
	///	<see cref="UnityEngine.SceneManagement.SceneManager.activeSceneChanged"/>,
	///	but catch all exceptions
	/// </summary>
	public static event Action<Scene, Scene> SceneChangeHook = null!;

	private static void OnSceneChange(Scene prev, Scene next) {
		Logger.LogFine($"{nameof(OnSceneChange)} Invoked");

		if (SceneChangeHook == null) {
			return;
		}

		foreach (Action<Scene, Scene> a in SceneChangeHook.GetInvocationList()) {
			try {
				a.Invoke(prev, next);
			} catch (Exception e) {
				Logger.LogError(e.ToString());
			}
		}
	}


	/// <param name="invincible">Invincibility state passed by vanilla code or last hooked method</param>
	/// <returns>New invincibility state</returns>
	public delegate bool HitEnemyHandler(HealthManager hm, HitInstance hit, bool invincible);

	/// <summary>
	/// Called when deciding whether an enemy is invincible to a hit or not
	/// </summary>
	public static event HitEnemyHandler HitEnemyHook = null!;

	private static bool OnHitEnemy(bool invincible, HealthManager hm, HitInstance hitInstance) {
		Logger.LogFine($"{nameof(OnHitEnemy)} Invoked");

		if (HitEnemyHook == null) {
			return invincible;
		}

		foreach (HitEnemyHandler a in HitEnemyHook.GetInvocationList()) {
			try {
				invincible = a.Invoke(hm, hitInstance, invincible);
			} catch (Exception e) {
				Logger.LogError(e.ToString());
			}
		}

		return invincible;
	}


	/// <summary>
	/// Called when game is about to pause <br />
	/// <see cref="Time.timeScale"/> is still 1f when called
	/// </summary>
	public static event Action GamePauseHook = null!;

	private static void OnGamePause() {
		Logger.LogFine($"{nameof(OnGamePause)} Invoked");

		if (GamePauseHook == null) {
			return;
		}

		foreach (Action a in GamePauseHook.GetInvocationList()) {
			try {
				a.Invoke();
			} catch (Exception e) {
				Logger.LogError(e.ToString());
			}
		}
	}

	/// <summary>
	/// Called when game is about to unpause <br />
	/// <see cref="Time.timeScale"/> is still 0f when called
	/// </summary>
	public static event Action GameUnpauseHook = null!;

	private static void OnGameUnpause() {
		Logger.LogFine($"{nameof(OnGameUnpause)} Invoked");

		if (GameUnpauseHook == null) {
			return;
		}

		foreach (Action a in GameUnpauseHook.GetInvocationList()) {
			try {
				a.Invoke();
			} catch (Exception e) {
				Logger.LogError(e.ToString());
			}
		}
	}


	static OsmiHooks() {
		GameInitializedHook += () => UIManager.EditMenus += OnMenuBuild;

		On.HeroController.Start += HookOnAfterEnterSave;

		USceneManager.activeSceneChanged += OnSceneChange;

		IL.HealthManager.Hit += il => new ILCursor(il)
			.Goto(0)
			.GotoNext(i => i.MatchCallvirt<HealthManager>(
				nameof(HealthManager.IsBlockingByDirection)
			))
			.Emit(OpCodes.Ldarg_0) // this
			.Emit(OpCodes.Ldarg_1) // hitInstance
			.EmitDelegate(OnHitEnemy);

		_ = new ILHook(
			ReflectionHelper
				.GetMethodInfo(typeof(GameManager), nameof(GameManager.PauseGameToggle))
				.GetStateMachineTarget(),
			il => new ILCursor(il)
				.Goto(0)
				.GotoNext(
					MoveType.Before,
					i => i.MatchLdcR4(0f),
					i => i.MatchCallvirt(typeof(GameManager), "SetTimeScale")
				)
				.Emit(OpCodes.Call, ((Delegate) OnGamePause).Method)
				.GotoNext(
					MoveType.Before,
					i => i.MatchLdcR4(1f),
					i => i.MatchCallvirt(typeof(GameManager), "SetTimeScale")
				)
				.Emit(OpCodes.Call, ((Delegate) OnGameUnpause).Method)
		);
	}
}
