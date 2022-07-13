using MonoMod.Utils;

using Osmi.Imports;

namespace Osmi.Game;

[PublicAPI]
public class SharedHealthManager : MonoBehaviour {
	internal const string SHM_FIELD = "sharedHealthManager";
	internal const string BACKED_SHM_FIELD = "backingSharedHealthManager";

	public HealthManager BackingHM { get; private init; }

	private readonly List<HealthManager> HMs = new();

	public IEnumerable<HealthManager> Members => HMs;

	public int HP {
		get => BackingHM.hp;
		set => BackingHM.hp = value;
	}

	#region Create

	public static SharedHealthManager Create(string name = "SharedHealthManager", int initialHP = 0) {
		GameObject go = new(name, typeof(SharedHealthManager));
		SharedHealthManager shm = go.GetComponent<SharedHealthManager>();
		shm.HP += initialHP;
		return shm;
	}

	public static SharedHealthManager Create(string name, int initialHP, IEnumerable<HealthManager> hms) {
		SharedHealthManager shm = Create(name, initialHP);
		shm.Add(hms);
		return shm;
	}

	public static SharedHealthManager Create(string name, IEnumerable<HealthManager> hms) =>
		Create(name, 0, hms);

	public static SharedHealthManager Create(string name, params HealthManager[] hms) =>
		Create(name, hms.AsEnumerable());

	#endregion

	private SharedHealthManager() {
		InstanceCount++;

		gameObject.layer = (int) PhysLayers.ENEMIES;

		BackingHM = gameObject.AddComponent<HealthManager>();
		BackingHM.OnDeath += KillMembers;
		new DynamicData(BackingHM).Set(BACKED_SHM_FIELD, this);
		gameObject.MarkAsBoss();

		// Tricks Debug Enemies Panel
		if (ModHooks.GetMod("DebugMod", true) != null) {
			_ = gameObject.AddComponent<NonBouncer>();
			gameObject.AddComponent<BoxCollider2D>().size =
				new(short.MaxValue, short.MaxValue);
		}
	}

	~SharedHealthManager() => InstanceCount--;

	public void OnDestroy() =>
		Die();

	#region Adding HM

	public void Add(HealthManager hm) {
		if (hm == BackingHM) {
			throw new InvalidOperationException("Cannot add own backing hm to shared hm");
		}

		DynamicData dyn = new(hm);
		if (dyn.Get<SharedHealthManager>(SHM_FIELD) != null) {
			throw new InvalidOperationException("HM already add to another shared HM");
		}

		HMs.Add(hm);
		hm.OnDeath += CheckDie;

		HP += hm.hp;
		hm.hp = int.MaxValue;

		dyn.Set(SHM_FIELD, this);

		RefreshEnemyHPBar(true);
	}

	public void Add(IEnumerable<HealthManager> hms) {
		foreach (HealthManager hm in hms) {
			Add(hm);
		}
	}

	public void Add(params HealthManager[] hms) => Add(hms.AsEnumerable());

	#endregion

	public void Hit(HitInstance hit) => BackingHM.Hit(hit with {
		AttackType = AttackTypes.RuinsWater,
		IgnoreInvulnerable = true
	});

	public void Remove(HealthManager hm, int healthTaking) {
		if (healthTaking < 0) {
			throw new ArgumentOutOfRangeException(nameof(healthTaking));
		}

		if (hm.GetSharedHM() != this) {
			throw new InvalidOperationException("Specified HM is not sharing in current Shared HM");
		}

		_ = HMs.Remove(hm);
		hm.hp = healthTaking;
		HP -= healthTaking;

		DynamicData dyn = new(hm);
		dyn.Set(SHM_FIELD, null);

		hm.gameObject.EnableHPBar();
	}

	public void CheckDie() {
		if (HMs.TrueForAll(hm => hm.isDead)) {
			Die();
		}
	}

	public void Die() =>
		BackingHM.Die(null, AttackTypes.RuinsWater, true);

	public void KillMembers() {
		foreach (HealthManager hm in HMs) {
			try {
				hm.Die(null, AttackTypes.Generic, true);
			} catch (NullReferenceException) {
			}
		}
	}

	public void RefreshEnemyHPBar(bool recheckMembers = false) {
		if (recheckMembers) {
			HMs.ForEach(hm => hm.gameObject.DisableHPBar());
		}

		gameObject.RefreshHPBar();
	}


	#region Damage Proxy

	private static readonly object lockObj = new();
	private static int instanceCount = 0;

	public static int InstanceCount {
		get => instanceCount;
		private set {
			if (value < 0) {
				throw new ArgumentOutOfRangeException(nameof(value));
			}

			lock (lockObj) {
				if (value == 0) {
					On.HealthManager.TakeDamage -= ReportDamage;
					On.HealthManager.ApplyExtraDamage -= ReportExtraDamage;
				} else if (instanceCount == 0 && value > 0) {
					On.HealthManager.TakeDamage += ReportDamage;
					On.HealthManager.ApplyExtraDamage += ReportExtraDamage;
				}

				instanceCount = value;
			}
		}
	}

	private static void ReportDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hit) {
		if (self.IsBackingHM()) {
			if (hit is {
				AttackType: AttackTypes.RuinsWater,
				IgnoreInvulnerable: true
			}) {
				orig(self, hit);
			}
		} else {
			orig(self, hit);
		}

		if (self.TryGetSharedHM(out SharedHealthManager? shm)) {
			shm.BackingHM.Hit(hit with {
				AttackType = AttackTypes.RuinsWater,
				IgnoreInvulnerable = true
			});
		}
	}

	private static void ReportExtraDamage(On.HealthManager.orig_ApplyExtraDamage orig, HealthManager self, int damage) {
		orig(self, damage);

		if (self.TryGetSharedHM(out SharedHealthManager? shm)) {
			shm.BackingHM.ApplyExtraDamage(damage);
		}
	}

	#endregion
}
