namespace Osmi;

public sealed class Osmi : Mod {
	public static Osmi? Instance { get; private set; }

	public static Osmi UnsafeInstance => Instance!;

	public static bool Active => Instance != null;

	public Osmi() =>
		Instance = this;
}
