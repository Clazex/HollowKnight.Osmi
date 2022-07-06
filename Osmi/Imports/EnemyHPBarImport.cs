using MonoMod.ModInterop;

namespace Osmi.Imports;

internal static class EnemyHPBar {
	[ModImportName(nameof(EnemyHPBar))]
	private static class EnemyHPBarImport {
		public static Action<GameObject>? DisableHPBar = null!;
		public static Action<GameObject>? EnableHPBar = null!;
		public static Action<GameObject>? RefreshHPBar = null!;
		public static Action<GameObject>? MarkAsBoss = null!;
	}

	static EnemyHPBar() =>
		typeof(EnemyHPBarImport).ModInterop();

	internal static void DisableHPBar(this GameObject self) =>
		EnemyHPBarImport.DisableHPBar?.Invoke(self);

	internal static void EnableHPBar(this GameObject self) =>
		EnemyHPBarImport.EnableHPBar?.Invoke(self);

	internal static void RefreshHPBar(this GameObject self) =>
		EnemyHPBarImport.RefreshHPBar?.Invoke(self);

	internal static void MarkAsBoss(this GameObject self) =>
		EnemyHPBarImport.MarkAsBoss?.Invoke(self);
}
