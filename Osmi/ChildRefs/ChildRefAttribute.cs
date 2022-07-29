namespace Osmi.ChildRefs;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
[PublicAPI]
[MeansImplicitUse]
public sealed class ChildRefAttribute : Attribute {
	public string Path { get; private init; }

	public ChildRefAttribute(string path = "") => Path = path;
}
