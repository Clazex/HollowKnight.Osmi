using MonoMod.ModInterop;

namespace Osmi.Exports;

internal static class ModInteropExports {
	internal static void Init() =>
		typeof(HealthShareExport).ModInterop();
}
