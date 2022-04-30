using System.Text;

namespace Osmi.Utils;

[PublicAPI]
public static class StringUtil {
	public static bool EnclosedWith(this string self, string start, string end) =>
		self.StartsWith(start) && self.EndsWith(end);

	public static string StripStart(this string self, string val) =>
		self.StartsWith(val) ? self.Substring(val.Length) : self;

	public static string StripEnd(this string self, string val) =>
		self.EndsWith(val) ? self.Substring(0, self.Length - val.Length) : self;


	public static string Repeat(this string self, int count) =>
		self.AsEnumerable().Repeat(count).Collect();

	public static string Concat(this string self, string other) =>
		self + other;


	public static string Slice(this string self, int startIndex) =>
		self.Slice(startIndex, self.Length);

	public static string Slice(this string self, int startIndex, int endIndex) {
		startIndex = startIndex >= 0 ? startIndex : startIndex + self.Length;
		endIndex = endIndex >= 0 ? endIndex : endIndex + self.Length;

		return self.Substring(startIndex, Math.Max(0, endIndex - startIndex));
	}


	public static string Remove(this string self, char val) =>
		self.Replace(val.ToString(), "");

	public static string Remove(this string self, string val) =>
		self.Replace(val, "");


	public static string Join(this IEnumerable<string> self, string sep) {
		using IEnumerator<string> enumerator = self.GetEnumerator();

		enumerator.MoveNext();
		StringBuilder builder = new(enumerator.Current);

		while (enumerator.MoveNext()) {
			builder.Append(sep);
			builder.Append(enumerator.Current);
		}

		return builder.ToString();
	}

	public static string Join(this IEnumerable<string> self, char sep) =>
		self.Join("" + sep);

	public static string Join(this IEnumerable<string> self) {
		using IEnumerator<string> enumerator = self.GetEnumerator();

		enumerator.MoveNext();
		StringBuilder builder = new(enumerator.Current);

		while (enumerator.MoveNext()) {
			builder.Append(enumerator.Current);
		}

		return builder.ToString();
	}


	public static bool IsNullOrEmpty(this string self) =>
		string.IsNullOrEmpty(self);

	public static bool IsNullOrWhiteSpace(this string self) =>
		string.IsNullOrWhiteSpace(self);
}
