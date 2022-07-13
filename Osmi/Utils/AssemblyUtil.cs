using System.Security.Cryptography;

namespace Osmi.Utils;

[PublicAPI]
public static class AssemblyUtil {
	/// <summary>
	///	Gets the version of the specified assembly
	/// </summary>
	/// <param name="self">The assembly to get version from</param>
	/// <returns>Version number in the format of X.X.X.X</returns>
	public static string GetVersion(this Assembly self) =>
		self.GetName().Version.ToString();

	/// <summary>
	///	Gets the informational version of the specified assembly,
	///	this differs from <see cref="GetVersion"/>
	///	as informational version has no format restriction
	/// </summary>
	/// <param name="self">The assembly to get informational version from</param>
	/// <returns>Informational version</returns>
	public static string GetInformationalVersion(this Assembly self) => self
		.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
		.InformationalVersion;


	/// <summary>
	///	Gets the path to the directory in which the specified assembly locates
	/// </summary>
	/// <param name="self">The assembly to get the directory</param>
	/// <returns>Path to the assembly's directory</returns>
	public static string GetDirectoryName(this Assembly self) =>
		Path.GetDirectoryName(self.Location);

	/// <summary>
	///	Reads the content of specified assembly as <see cref="FileStream"/>
	/// </summary>
	/// <param name="self">The assembly to read from</param>
	/// <returns>The <see cref="FileStream"/> containing the assembly's content</returns>
	public static FileStream ReadAsStream(this Assembly self) =>
		File.OpenRead(self.Location);


	/// <summary>
	///	Gets the <see cref="SHA1"/> hash of specified assembly
	/// </summary>
	/// <param name="self">The assembly to get hash from</param>
	/// <param name="length">Length of hash string to be preserved</param>
	/// <returns>The <see cref="SHA1"/> hash string</returns>
	public static string GetHash(this Assembly self, int length = 7) {
		using var hasher = SHA1.Create();
		byte[] bytes = File.ReadAllBytes(Assembly.GetExecutingAssembly().Location);
		return BitConverter.ToString(hasher.ComputeHash(bytes))
			.Replace("-", "").Substring(0, length).ToLowerInvariant();
	}

	/// <summary>
	///	Gets the version with <see cref="SHA1"/> hash of specified assembly,
	///	combination of <see cref="GetVersion"/> and <see cref="GetHash"/>
	/// </summary>
	/// <param name="self">The assembly to get version and hash from</param>
	/// <param name="length">Length of hash string to be preserved</param>
	/// <returns></returns>
	public static string GetVersionWithHash(this Assembly self, int length = 7) =>
		$"{self.GetVersion()}+{self.GetHash(length)}";

	/// <summary>
	///	Gets the informational version with <see cref="SHA1"/> hash of specified assembly,
	///	combination of <see cref="GetInformationalVersion"/> and <see cref="GetHash"/>
	/// </summary>
	/// <param name="self">The assembly to get informational version and hash from</param>
	/// <param name="length">Length of hash string to be preserved</param>
	/// <returns></returns>
	public static string GetInformationalVersionWithHash(this Assembly self, int length = 7) =>
		$"{self.GetInformationalVersion()}+{self.GetHash(length)}";


	/// <inheritdoc cref="GetVersion" />
	public static string GetMyVersion() =>
		Assembly.GetCallingAssembly().GetVersion();

	/// <inheritdoc cref="GetInformationalVersion" />
	public static string GetMyInformationalVersion() =>
		Assembly.GetCallingAssembly().GetInformationalVersion();

	/// <inheritdoc cref="GetHash"/>
	public static string GetMyHash(int length = 7) =>
		Assembly.GetCallingAssembly().GetHash(length);

	/// <inheritdoc cref="GetVersionWithHash"/>
	public static string GetMyVersionWithHash(int length = 7) =>
		Assembly.GetCallingAssembly().GetVersionWithHash(length);

	/// <inheritdoc cref="GetInformationalVersionWithHash"/>
	public static string GetMyInformationalVersionWithHash(int length = 7) =>
		Assembly.GetCallingAssembly().GetInformationalVersionWithHash(length);


	/// <inheritdoc cref="GetInformationalVersion"/>
	public static Lazy<string> GetMyDefaultVersion() {
		var asm = Assembly.GetCallingAssembly();
		return new(() => asm.GetInformationalVersion());
	}

	/// <inheritdoc cref="GetInformationalVersionWithHash"/>
	public static Lazy<string> GetMyDefaultVersionWithHash() {
		var asm = Assembly.GetCallingAssembly();
		return new(() => asm.GetInformationalVersionWithHash());
	}


	/// <summary>
	///	Gets all manifest resource streams of specified assembly
	/// </summary>
	/// <param name="self">The assembly to get manifest resource streams from</param>
	/// <returns>Manifest resource streams</returns>
	public static IEnumerable<Stream> GetManifestResourceStreams(this Assembly self) => self
		.GetManifestResourceStreams(Delegates.Pass<string>());

	/// <inheritdoc cref="GetManifestResourceStreams(Assembly)"/>
	/// <param name="self">The assembly to get manifest resource streams from</param>
	/// <param name="f">A delegate to filter the resources</param>
	public static IEnumerable<Stream> GetManifestResourceStreams(
		this Assembly self, Func<string, bool> f
	) => self
		.GetManifestResourceNames()
		.Filter(f)
		.Map(self.GetManifestResourceStream);
}
