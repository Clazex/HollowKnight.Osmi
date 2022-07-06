using MonoMod.ModInterop;

using Osmi.Game;

namespace Osmi.Exports;

[ModExportName("HealthShare")]
public static class HealthShareExport {
	public static bool IsSharing(HealthManager self) => self.IsSharing();

	public static bool IsBackingHM(HealthManager self) => self.IsBackingHM();

	public static void AddToShared(HealthManager self, SharedHealthManager shm) =>
		self.AddToShared(shm);

	public static void AddToShared(IEnumerable<HealthManager> self, SharedHealthManager shm) =>
		self.AddToShared(shm);

	public static void AddToShared(GameObject self, SharedHealthManager shm) =>
		self.AddToShared(shm);

	public static void AddToShared(IEnumerable<GameObject> self, SharedHealthManager shm) =>
		self.AddToShared(shm);

	public static MonoBehaviour ShareHealth(IEnumerable<HealthManager> self, int initialHP, string name) =>
		self.ShareHealth(initialHP, name);

	public static MonoBehaviour ShareHealth(IEnumerable<GameObject> self, int initialHP, string name) =>
		self.ShareHealth(initialHP, name);

	public static MonoBehaviour ShareWith(HealthManager self, params HealthManager[] hms) =>
		self.ShareWith(hms);

	public static void StopSharing(HealthManager self, int healthTaking) =>
		self.StopSharing(healthTaking);
}
