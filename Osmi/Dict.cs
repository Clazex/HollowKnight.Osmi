using Language;

namespace Osmi;

[PublicAPI]
public class Dict {
	private readonly Dictionary<string, Lazy<Dictionary<string, string>>> dict;

	public Dict(Assembly asm, string path) {
		string prefix = asm.GetName().Name + '.' + path + '.';
		dict = LangUtil.LoadLangs(
			asm,
			asm.GetManifestResourceNames()
			.Filter(name => {
				if (!name.StartsWith(prefix)) {
					return false;
				}

				string[] segments = name.StripStart(prefix).Split('.');
				return segments.Length == 2 && segments[1] == "json" && LangUtil.Langs.Contains(segments[0]);
			})
			.Map(name => (
				name.StripStart(prefix).StripEnd(".json"),
				name
			))
		);
	}

	public string Localize(string key) {
		_ = dict.TryGetValue(LangUtil.CurrentLang, out Lazy<Dictionary<string, string>>? table);
		table ??= dict[LanguageCode.EN.ToIdentifier()];
		return table.Value.TryGetValue(key, out string value) ? value : key;
	}
}
