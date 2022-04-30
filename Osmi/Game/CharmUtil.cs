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
		if (EquippedCharm(charm) || Ref.PD.charmSlotsFilled >= Ref.PD.charmSlots) {
			return false;
		}

		Ref.PD.charmSlotsFilled += GetCharmCost(charm);

		if (Ref.PD.charmSlotsFilled > Ref.PD.charmSlots) {
			Ref.PD.canOvercharm = true;
			Ref.PD.overcharmed = true;
		}

		UpdateCharm();

		return true;
	}

	public static bool UnequipCharm(int charm) {
		if (!EquippedCharm(charm)) {
			return false;
		}

		Ref.PD.charmSlotsFilled -= GetCharmCost(charm);

		if (Ref.PD.overcharmed && Ref.PD.charmSlotsFilled <= Ref.PD.charmSlots) {
			Ref.PD.overcharmed = false;
		}

		Ref.PD.SetBool($"equippedCharm_{charm}", false);
		Ref.PD.UnequipCharm(charm);

		UpdateCharm();

		return true;
	}


	public static bool GotCharm(Charm charm) => GotCharm((int) charm);


	public static bool EquippedCharm(Charm charm) => EquippedCharm((int) charm);

	public static int GetCharmCost(Charm charm) => GetCharmCost((int) charm);

	public static bool EquipCharm(Charm charm) => EquipCharm((int) charm);

	public static bool UnequipCharm(Charm charm) => UnequipCharm((int) charm);
}
