namespace Osmi.Utils;

[PublicAPI]
public static class CloneUtil {
	public static void MemberwiseCopy<T>(Type t, T source, ref T target) where T : class {
		foreach (FieldInfo fi in t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
			fi.SetValue(target, fi.GetValue(source));
		}
	}

	public static void MemberwiseCopy<T>(T source, ref T target) where T : class =>
		MemberwiseCopy(source.GetType(), source, ref target);

	public static T CreateMemberwiseClone<T>(T source) where T : class {
		var target = (T) Activator.CreateInstance(source.GetType(), true);
		MemberwiseCopy(source, ref target);
		return target;
	}
}
