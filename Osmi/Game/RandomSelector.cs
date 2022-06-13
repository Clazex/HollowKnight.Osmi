namespace Osmi.Game;

// Simulates SendRandomEventV3
[PublicAPI]
public class RandomSelector<T> {
	private readonly List<SelectionItem> items;

	public int ItemCount => items.Count;


	public RandomSelector() : this(new()) { }

	public RandomSelector(List<SelectionItem> items) => this.items = items;

	public void AddItem(SelectionItem item) => items.Add(item);

	public void AddItems(IEnumerable<SelectionItem> items) => items.ForEach(AddItem);

	public bool RemoveItem(SelectionItem item) => items.Remove(item);

	public void ClearItem() => items.Clear();


	public T Select() {
		if (items.Count == 0) {
			throw new InvalidOperationException("Selection item list is empty");
		}

		lock (items) {
			SelectionItemInfo[] info = items.Map(SelectionItem.Pin).ToArray();
			float totalWeight = info.Reduce((a, i) => a + i.weight, 0f);

			for (int i = 0; i < 100; i++) {
				float rng = UnityEngine.Random.Range(0f, totalWeight);
				SelectionItemInfo? selected = info.FirstOrDefault(i => {
					if (rng < i.weight) {
						return true;
					}

					rng -= i.weight;
					return false;
				});

				if (selected == null) {
					continue;
				}

				if (info.LastOrDefault(
					 item => item.item.missed >= item.missedMax && item.missedMax > 0
				) is SelectionItemInfo missed) {
					items.ForEach(item => {
						item.combo = 0;
						item.missed++;
					});

					SelectionItem item = missed.item;
					item.combo = 1;
					item.missed = 0;
					return item.value;
				} else {
					if (selected.item.combo >= selected.comboMax && selected.comboMax > 0) {
						continue;
					}

					int combo = selected.item.combo + 1;
					items.ForEach(item => {
						item.combo = 0;
						item.missed++;
					});

					SelectionItem item = selected.item;
					item.combo = combo;
					item.missed = 0;
					return item.value;
				}
			}

			return items[0].value;
		}
	}

	public sealed class SelectionItem {
		public T value;

		public Func<float> weight;

		public Func<int> comboMax;
		internal int combo = 0;

		public Func<int> missedMax;
		internal int missed = 0;

		public SelectionItem(T value, float weight = 1, int comboMax = 0, int missedMax = 0)
			: this(value, Funcs.Constant(weight), Funcs.Constant(comboMax), Funcs.Constant(missedMax)) {
		}

		public SelectionItem(T value)
			: this(value, Funcs.Constant(1f)) {
		}

		public SelectionItem(T value, Func<float> weight)
			: this(value, weight, Funcs.Constant(0)) {
		}

		public SelectionItem(T value, Func<float> weight, Func<int> comboMax)
			: this(value, weight, comboMax, Funcs.Constant(0)) {
		}

		public SelectionItem(T value, Func<float> weight, Func<int> comboMax, Func<int> missedMax) {
			this.value = value;
			this.weight = weight;
			this.comboMax = comboMax;
			this.missedMax = missedMax;
		}

		internal static SelectionItemInfo Pin(SelectionItem item) =>
			new(item, item.weight(), item.comboMax(), item.missedMax());
	}

	internal class SelectionItemInfo {
		internal readonly SelectionItem item;
		internal readonly float weight;
		internal readonly int comboMax;
		internal readonly int missedMax;

		internal SelectionItemInfo(SelectionItem item, float weight, int comboMax, int missedMax) {
			this.item = item;
			this.weight = weight;
			this.comboMax = comboMax;
			this.missedMax = missedMax;
		}
	}
}
