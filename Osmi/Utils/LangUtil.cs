using Language;

namespace Osmi.Utils;

[PublicAPI]
public static class LangUtil {
	public static HashSet<string> Langs { get; } = Lang
		.GetLanguages()
		.Map(str => str.ToLower().Replace('_', '-'))
		.ToHashSet();

	public static string ToIdentifier(this LanguageCode code) =>
		code.ToString().ToLower().Replace('_', '-');

	public static string CurrentLang =>
		Lang.CurrentLanguage().ToIdentifier();

	public static Dictionary<string, Lazy<Dictionary<string, string>>> LoadLangs(
		Assembly asm,
		IEnumerable<(string lang, string path)> tuples
	) => tuples.ToDictionary(
		tuple => tuple.lang,
		tuple => new Lazy<Dictionary<string, string>>(() => {
			string content = asm
				.GetManifestResourceStream(tuple.path).ReadToString();
			Dictionary<string, string> table =
				JsonConvert.DeserializeObject<Dictionary<string, string>>(content)!;

			Logger.LogDebug($"Loaded localization for lang {tuple.lang} in {asm.GetName().Name}");
			return table;
		})
	);
}
