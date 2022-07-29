namespace Osmi.SimpleFSM;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
[PublicAPI]
[MeansImplicitUse]
public sealed class FSMStateAttribute : Attribute {
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
[PublicAPI]
public sealed class FSMGlobalTransitionAttribute : Attribute {
	public string OnEvent { get; private init; }

	public FSMGlobalTransitionAttribute(string onEvent) =>
		OnEvent = onEvent;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
[PublicAPI]
public sealed class FSMTransitionAttribute : Attribute {
	public string OnEvent { get; private init; }

	public string ToState { get; private init; }

	public FSMTransitionAttribute(string onEvent, string toState) {
		OnEvent = onEvent;
		ToState = toState;
	}
}
