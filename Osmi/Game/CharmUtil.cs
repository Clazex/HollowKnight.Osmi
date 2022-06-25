namespace Osmi.Game;

[PublicAPI]
public static class CharmUtil {
	public static void UpdateCharm() {
		Ref.HC.CharmUpdate();
		PlayMakerFSM.BroadcastEvent("CHARM INDICATOR CHECK");
		PlayMakerFSM.BroadcastEvent("UPDATE BLUE HEALTH");
	}


	public static bool GotCharm(int charm) =>
		Ref.PD.GetBool($"gotCharm_{charm}");

	public static bool EquippedCharm(int charm) =>
		Ref.PD.GetBool($"equippedCharm_{charm}");

	public static int GetCharmCost(int charm) =>
		Ref.PD.GetInt($"charmCost_{charm}");

	public static bool EquipCharm(int charm) {
		if (EquippedCharm(charm) || Ref.PD.GetInt("charmSlotsFilled") >= Ref.PD.GetInt("charmSlots")) {
			return false;
		}

		Ref.PD.SetInt("charmSlotsFilled", Ref.PD.GetInt("charmSlotsFilled") + GetCharmCost(charm)) ;

		if (Ref.PD.GetInt("charmSlotsFilled") > Ref.PD.GetInt("charmSlots")) {
			Ref.PD.SetBool("canOvercharm", true);
			Ref.PD.SetBool("overcharmed", true);
		}

		UpdateCharm();

		return true;
	}

	public static bool UnequipCharm(int charm) {
		if (!EquippedCharm(charm)) {
			return false;
		}

		Ref.PD.SetInt("charmSlotsFilled", Ref.PD.GetInt("charmSlotsFilled") - GetCharmCost(charm));

		if (Ref.PD.GetBool("overcharmed") && Ref.PD.GetInt("charmSlotsFilled") <= Ref.PD.GetInt("charmSlots")) {
			Ref.PD.SetBool("overcharmed", false);
		}

		Ref.PD.SetBool($"equippedCharm_{charm}", false);
		Ref.PD.UnequipCharm(charm);

		UpdateCharm();

		return true;
	}

	public static void UnequipAllCharms() =>
		Ref.PD.GetVariable<List<int>>("equippedCharms").ToArray().ForEach(i => UnequipCharm(i));


	public static bool GotCharm(Charm charm) => GotCharm((int) charm);


	public static bool EquippedCharm(Charm charm) => EquippedCharm((int) charm);

	public static int GetCharmCost(Charm charm) => GetCharmCost((int) charm);

	public static bool EquipCharm(Charm charm) => EquipCharm((int) charm);

	public static bool UnequipCharm(Charm charm) => UnequipCharm((int) charm);
}
