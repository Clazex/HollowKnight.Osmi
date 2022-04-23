namespace Osmi;

public sealed class Osmi : Mod {
	public static Osmi? Instance { get; private set; }

	public static Osmi UnsafeInstance => Instance!;

	public static bool Active => Instance != null;

	public static Lazy<string> version = AssemblyUtil
#if DEBUG
		.GetMyDefaultVersionWithHash();
#else
		.GetMyDefaultVersion();
#endif

	public override string GetVersion() => version.Value;

	public Osmi() =>
		Instance = this;
}
