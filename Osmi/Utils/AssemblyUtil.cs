using System.IO;
using System.Security.Cryptography;

namespace Osmi.Utils;

[PublicAPI]
public static class AssemblyUtil {
	public static string GetVersion(this Assembly self) =>
		self.GetName().Version.ToString();

	public static string GetInformationalVersion(this Assembly self) => self
		.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
		.InformationalVersion;


	public static string GetDirectoryName(this Assembly self) =>
		Path.GetDirectoryName(self.Location);

	public static FileStream ReadAsStream(this Assembly self) =>
		File.OpenRead(self.Location);


	public static string GetHash(this Assembly self, int length = 7) {
		using var hasher = SHA1.Create();
		byte[] bytes = File.ReadAllBytes(Assembly.GetExecutingAssembly().Location);
		return BitConverter.ToString(hasher.ComputeHash(bytes))
			.Replace("-", "").Substring(0, length).ToLowerInvariant();
	}

	public static string GetVersionWithHash(this Assembly self, int length = 7) =>
		$"{self.GetVersion()}+{self.GetHash(length)}";

	public static string GetInformationalVersionWithHash(this Assembly self, int length = 7) =>
		$"{self.GetInformationalVersion()}+{self.GetHash(length)}";


	public static string GetMyVersion() =>
		Assembly.GetCallingAssembly().GetVersion();

	public static string GetMyInformationalVersion() =>
		Assembly.GetCallingAssembly().GetInformationalVersion();

	public static string GetMyHash(int length = 7) =>
		Assembly.GetCallingAssembly().GetHash(length);

	public static string GetMyVersionWithHash(int length = 7) =>
		Assembly.GetCallingAssembly().GetVersionWithHash(length);

	public static string GetMyInformationalVersionWithHash(int length = 7) =>
		Assembly.GetCallingAssembly().GetInformationalVersionWithHash(length);


	public static Lazy<string> GetMyDefaultVersion() {
		var asm = Assembly.GetCallingAssembly();
		return new(() => asm.GetInformationalVersion());
	}

	public static Lazy<string> GetMyDefaultVersionWithHash() {
		var asm = Assembly.GetCallingAssembly();
		return new(() => asm.GetInformationalVersionWithHash());
	}


	public static IEnumerable<Stream> GetManifestResourceStreams(this Assembly self) => self
		.GetManifestResourceStreams(Delegates.Pass<string>());

	public static IEnumerable<Stream> GetManifestResourceStreams(
		this Assembly self, Func<string, bool> f
	) => self
		.GetManifestResourceNames()
		.Filter(f)
		.Map(self.GetManifestResourceStream);
}
