namespace Osmi.Game;

// Simulates SendRandomEventV3
[PublicAPI]
public class RandomSelector<T> {
	private readonly List<SelectionItem> items;

	private bool itemDataFresh = false;

	public int ItemCount => items.Count;

	private float TotalWeight { get; set; } = 0;


	public RandomSelector() : this(new()) { }

	public RandomSelector(List<SelectionItem> items) =>
		this.items = items;

	public void AddItem(SelectionItem item) {
		items.Add(item);
		itemDataFresh = false;
	}

	public void AddItems(IEnumerable<SelectionItem> items) => items.ForEach(AddItem);

	public void RemoveItem(SelectionItem item) {
		_ = items.Remove(item);
		itemDataFresh = false;
	}

	public void ClearItem() {
		items.Clear();
		itemDataFresh = false;
	}

	private void UpdateItemData() {
		float totalWeight = 0;
		foreach (SelectionItem item in items) {
			totalWeight += item.weight;
			item.cumulativeWeight = totalWeight;
		}

		TotalWeight = totalWeight;
		itemDataFresh = true;
	}


	public T Select() {
		lock (items) {
			if (items.Count == 0) {
				throw new InvalidOperationException("Selection item list is empty");
			}

			if (!itemDataFresh) {
				UpdateItemData();
			}

			for (int i = 0; i < 100; i++) {
				if (items.FirstOrDefault(item => item.missed >= item.missedMax && item.missedMax > 0) is SelectionItem missed) {
					items.ForEach(item => {
						item.combo = 0;
						item.missed++;
					});

					missed.combo = 1;
					missed.missed = 0;
					return missed.value;
				}

				float rng = UnityEngine.Random.Range(0, TotalWeight);
				if (items.FirstOrDefault(item => item.cumulativeWeight > rng) is SelectionItem selected) {
					if (selected.combo >= selected.comboMax && selected.comboMax > 0) {
						continue;
					}

					int combo = selected.combo + 1;
					items.ForEach(item => {
						item.combo = 0;
						item.missed++;
					});
					selected.combo = combo;
					selected.missed = 0;

					return selected.value;
				}
			}

			return items[0].value;
		}
	}

	public class SelectionItem {
		public T value;

		public float weight;
		internal float cumulativeWeight = 0;

		public int comboMax;
		internal int combo = 0;

		public int missedMax;
		internal int missed = 0;

		public SelectionItem(T value, float weight = 1, int comboMax = 0, int missedMax = 0) {
			this.value = value;
			this.weight = weight;
			this.comboMax = comboMax;
			this.missedMax = missedMax;
		}
	}
}
