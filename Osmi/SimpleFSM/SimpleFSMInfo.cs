using MonoMod.Utils;

using Osmi.ChildRefs;

namespace Osmi.SimpleFSM;

[PublicAPI]
public sealed class SimpleFSMInfo {
	internal static readonly LazyMap<Type, SimpleFSMInfo> map = new(New);

	private static SimpleFSMInfo New(Type t) => new(t);

	private SimpleFSMInfo(Type t) {
		if (t == typeof(SimpleFSM)) {
			throw new InvalidOperationException("Cannot compile SimpleFSM base class");
		}

		if (t.IsAbstract) {
			throw new InvalidOperationException($"Class {t.FullName} is abstract");
		}

		if (!t.IsSubclassOf(typeof(SimpleFSM))) {
			throw new InvalidOperationException($"Class {t.FullName} is not {nameof(SimpleFSM)}");
		}

		List<Exception> exceptions = new();

		IEnumerable<MethodInfo> stateMethods = t
			.GetRuntimeMethods()
			.Filter(m => Attribute.IsDefined(m, typeof(FSMStateAttribute)))
			.Filter(m => {
				if (!typeof(IEnumerator).IsAssignableFrom(m.ReturnType)) {
					exceptions.Add(new InvalidOperationException(
						$"Method {m.Name} in SimpleFSM class {t.FullName} is marked" +
						$" as state method but does not return an {nameof(IEnumerator)}"
					));
					return false;
				}

				return true;
			});

		states = stateMethods.ToDictionary(
			m => m.Name,
			m => m.GetFastDelegate()
		);

		stateMethods.ForEach(m => {
			string name = m.Name;
			m.GetCustomAttributes<FSMGlobalTransitionAttribute>().ForEach(attr => {
				if (!globalTransitions.TryAdd(attr.OnEvent, name)) {
					exceptions.Add(new InvalidOperationException(
						  $"Duplicate global transition on event {attr.OnEvent}, existed on state "
						  + $"{globalTransitions[attr.OnEvent]}, but found another one on state {name}"
					));
				}
			});

			Dictionary<string, string> currentTransitions = new();
			transitions[name] = currentTransitions;
			m.GetCustomAttributes<FSMTransitionAttribute>().ForEach(attr => {
				if (!states.Keys.Contains(attr.ToState)) {
					exceptions.Add(new InvalidOperationException(
						$"Invalid state name {attr.ToState} on event {attr.OnEvent} found on state {m.Name}"
					));
				} else if (!currentTransitions.TryAdd(attr.OnEvent, attr.ToState)) {
					exceptions.Add(new InvalidOperationException(
						  $"Duplicate transition on event {attr.OnEvent} to {attr.ToState} found on "
						  + $"state {m.Name}, one to {currentTransitions[attr.OnEvent]} already exists"
					));
				}
			});
		});


		ChildRef.Precompile(t);


		if (exceptions.Count == 1) {
			throw exceptions[0];
		} else if (exceptions.Count > 1) {
			throw new AggregateException(exceptions);
		}
	}

	private readonly Dictionary<string, FastReflectionDelegate> states;

	private readonly Dictionary<string, string> globalTransitions = new();

	private readonly Dictionary<string, IReadOnlyDictionary<string, string>> transitions = new();

	internal IReadOnlyDictionary<string, FastReflectionDelegate> States => states;


	public IReadOnlyCollection<string> StateNames => states.Keys;

	public IReadOnlyDictionary<string, string> GlobalTransitions => globalTransitions;

	public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Transitions => transitions;
}
