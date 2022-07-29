using System.Reflection.Emit;

using MonoMod.Utils;

namespace Osmi.ChildRefs;

[PublicAPI]
public static class ChildRef {
	internal static readonly LazyMap<Type, Action<Component>> map = new(Compile);

	private static readonly MethodInfo componentGetComponent = typeof(Component)
		.GetMethod(nameof(Component.GetComponent), Type.EmptyTypes);
	private static readonly MethodInfo componentTfGetter = ReflectionHelper
		.GetPropertyInfo(typeof(Component), nameof(Component.transform)).GetMethod;
	private static readonly MethodInfo tfFind = ReflectionHelper
		.GetMethodInfo(typeof(Transform), nameof(Transform.Find));
	private static readonly MethodInfo tfGOGetter = ReflectionHelper
		.GetPropertyInfo(typeof(Transform), nameof(Transform.gameObject)).GetMethod;

	private static readonly LazyMap<Type, MethodInfo> genericComponentGetComponent =
		new(t => componentGetComponent.MakeGenericMethod(t));


	public static void Precompile<T>() where T : Component => _ = map[typeof(T)];

	public static void Precompile(Type t) => _ = map[t];


	private static Action<Component> Compile(Type t) {
		if (t.IsAbstract) {
			throw new InvalidOperationException($"Class {t.FullName} is abstract");
		}

		if (!t.IsSubclassOf(typeof(Component))) {
			throw new NotSupportedException($"Class {t.FullName} is not {nameof(Component)}");
		}

		List<Exception> exceptions = new();


		DynamicMethodDefinition dmd = new(t.Name + "ChildRefsInit", typeof(void), new[] { typeof(Component) });
		ILGenerator ilg = dmd.GetILGenerator();

		foreach (FieldInfo fi in t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
			if (fi.GetCustomAttribute<ChildRefAttribute>() is not ChildRefAttribute attr) {
				continue;
			}

			string path = attr.Path;
			Type type = fi.FieldType;

			if (type != typeof(GameObject) && !type.IsSubclassOf(typeof(Component))) {
				exceptions.Add(new InvalidOperationException(
					  $"Type {type.FullName} is neither GameObject nor Component"
				));
				continue;
			}

			if (path.Length == 0) {
				if (type == typeof(GameObject)) {
					exceptions.Add(new InvalidOperationException(
						  "Do not use Child Ref to get the GameObject of its own"
					));
					continue;
				}

				if (type == typeof(Transform)) {
					exceptions.Add(new InvalidOperationException(
						  "Do not use Child Ref to get the Transform of its own"
					));
					continue;
				}
			}


			ilg.Emit(OpCodes.Ldarg_0);
			ilg.Emit(OpCodes.Ldarg_0);

			if (path.Length == 0) {
				ilg.Emit(OpCodes.Call, genericComponentGetComponent[type]);
			} else {
				ilg.Emit(OpCodes.Call, componentTfGetter);
				ilg.Emit(OpCodes.Ldstr, path);
				ilg.Emit(OpCodes.Call, tfFind);

				if (type == typeof(GameObject)) {
					ilg.Emit(OpCodes.Call, tfGOGetter);
				} else if (type == typeof(Transform)) {
					// Sets with transform itself
				} else {
					ilg.Emit(OpCodes.Call, genericComponentGetComponent[type]);
				}
			}

			ilg.Emit(OpCodes.Stfld, fi);
		}

		ilg.Emit(OpCodes.Ret);


		if (exceptions.Count == 1) {
			throw exceptions[0];
		} else if (exceptions.Count > 1) {
			throw new AggregateException(exceptions);
		}

		return dmd.Generate().CreateDelegate<Action<Component>>();
	}
}
