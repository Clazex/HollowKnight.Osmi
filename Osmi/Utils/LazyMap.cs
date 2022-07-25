namespace Osmi.Utils;

public class LazyMap<K, V> {
	private readonly Func<K, V> mapper;
	private readonly Dictionary<K, V> inner = new();

	public LazyMap(Func<K, V> mapper) => this.mapper = mapper;

	public V this[K key] {
		get {
			lock (inner) {
				return inner.TryGetValue(key, out V value) ? value : inner[key] = mapper.Invoke(key);
			}
		}
	}
}
