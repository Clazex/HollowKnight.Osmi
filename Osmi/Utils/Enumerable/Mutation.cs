namespace Osmi.Utils;

public static partial class EnumerableUtil {
	public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> self) =>
		self.FlatMap(Funcs.Identity);

	public static IEnumerable<T> Repeat<T>(this IEnumerable<T> self, int times) {
		for (int i = 0; i < times; i++) {
			foreach (T t in self) {
				yield return t;
			}
		}
	}


	public static IEnumerable<T> RemoveAt<T>(this IEnumerable<T> self, int index) {
		(IEnumerable<T> former, _, IEnumerable<T> latter) = self.SplitAt(index);
		return former.Concat(latter);
	}

	public static IEnumerable<T> Insert<T>(this IEnumerable<T> self, int index, T value) {
		if (index < 0 || index >= self.Count()) {
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		(IEnumerable<T> former, IEnumerable<T> latter) = self.Split(index);
		return former.Append(value).Concat(latter);
	}


	public static IEnumerable<T> Order<T, U>(this IEnumerable<T> self) =>
		self.OrderBy(Funcs.Identity);

	public static IEnumerable<T> OrderDescending<T, U>(this IEnumerable<T> self) =>
		self.OrderByDescending(Funcs.Identity);


	/// <inheritdoc cref="Shuffle{T}(IEnumerable{T}, System.Random)"/>
	/// <remarks>Uses <see cref="UnityEngine.Random.Range(int, int)"/> to create random seed</remarks>
	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self) =>
		self.Shuffle(new(UnityEngine.Random.Range(int.MinValue, int.MaxValue)));

	/// <summary>
	/// <para>
	/// Uses Durstenfeld version of
	/// <seealso href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm">Fisher–Yates shuffle</seealso>,
	/// or Knuth shuffle, to shuffle the enumerable
	/// </para>
	/// <para>
	/// The following snippet is a naive implementation of shuffle algorithm:
	/// <code>
	/// System.Random random = new();
	/// enumerable.OrderBy(i => random.Next());
	/// </code>
	/// It is simple, yet not performant, and is biased. So always prefer this implementation over that one.
	/// </para>
	/// <para>
	/// Further reading: <seealso href="https://blog.codinghorror.com/the-danger-of-naivete/">The Danger of Naïveté</seealso>
	/// </para>
	/// </summary>
	/// <param name="self" />
	/// <param name="random">Random number generator</param>
	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self, System.Random random) {
		T[] array = self.ToArray();
		int count = array.Length;
		for (int i = 0; i < count; i++) {
			int j = random.Next(i, count);
			yield return array[j];
			array[j] = array[i];
		}
	}
}
