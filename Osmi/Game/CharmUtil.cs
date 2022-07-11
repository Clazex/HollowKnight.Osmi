namespace Osmi.Game;

[PublicAPI]
public static class CharmUtil {
	public static GameObject CharmsPane => Ref.GC.gameObject.Child("HudCamera", "Inventory", "Charms")!;
	public static bool CharmsPaneOpen => CharmsPane.Child("Cursor", "Back")!.activeInHierarchy;


	public static void UpdateCharm() {
		Ref.HC.CharmUpdate();
		PlayMakerFSM.BroadcastEvent("CHARM INDICATOR CHECK");
		PlayMakerFSM.BroadcastEvent("UPDATE BLUE HEALTH");
		UpdateCharmUI();
	}


	public static bool GotCharm(int charm) =>
		Ref.PD.GetBool($"gotCharm_{charm}");

	public static bool EquippedCharm(int charm) =>
		Ref.PD.GetBool($"equippedCharm_{charm}");

	public static int GetCharmCost(int charm) =>
		Ref.PD.GetInt($"charmCost_{charm}");

	public static bool EquipCharm(int charm) => EquipCharmInternal(Ref.PD, charm);

	public static bool UnequipCharm(int charm) => UnequipCharmInternal(Ref.PD, charm);

	public static bool EquipCharms(params int[] charms) {
		PlayerData pd = Ref.PD;

		foreach (int charm in charms) {
			if (!EquipCharmInternal(pd, charm)) {
				return false;
			}
		}

		return true;
	}

	public static bool UnequipCharms(params int[] charms) {
		PlayerData pd = Ref.PD;

		foreach (int charm in charms) {
			if (!UnequipCharmInternal(pd, charm)) {
				return false;
			}
		}

		return true;
	}

	public static void UnequipAllCharms() =>
		UnequipCharms(Ref.PD.GetVariable<List<int>>(nameof(PlayerData.equippedCharms)).ToArray());



	public static bool GotCharm(Charm charm) => GotCharm((int) charm);

	public static bool EquippedCharm(Charm charm) => EquippedCharm((int) charm);

	public static int GetCharmCost(Charm charm) => GetCharmCost((int) charm);

	public static bool EquipCharm(Charm charm) => EquipCharm((int) charm);

	public static bool UnequipCharm(Charm charm) => UnequipCharm((int) charm);

	public static bool EquipCharms(params Charm[] charms) => EquipCharms(charms.Map(i => (int) i).ToArray());

	public static bool UnequipCharms(params Charm[] charms) => UnequipCharms(charms.Map(i => (int) i).ToArray());


	private static bool EquipCharmInternal(PlayerData pd, int charm) {
		if (EquippedCharm(charm) || pd.GetInt(nameof(PlayerData.charmSlotsFilled)) >= pd.GetInt(nameof(PlayerData.charmSlots))) {
			return false;
		}

		pd.IntAdd(nameof(PlayerData.charmSlotsFilled), GetCharmCost(charm));

		if (pd.GetInt(nameof(PlayerData.charmSlots)) < pd.GetInt(nameof(PlayerData.charmSlotsFilled))) {
			pd.SetBool(nameof(PlayerData.canOvercharm), true);
			pd.SetBool(nameof(PlayerData.overcharmed), true);
		}

		pd.SetBool($"equippedCharm_{charm}", true);
		pd.EquipCharm(charm);

		return true;
	}


	private static bool UnequipCharmInternal(PlayerData pd, int charm) {
		if (!EquippedCharm(charm)) {
			return false;
		}

		pd.IntAdd(nameof(PlayerData.charmSlotsFilled), -GetCharmCost(charm));

		if (pd.GetBool(nameof(PlayerData.overcharmed)) && pd.GetInt(nameof(PlayerData.charmSlotsFilled)) <= pd.GetInt(nameof(PlayerData.charmSlots))) {
			pd.SetBool(nameof(PlayerData.overcharmed), false);
		}

		pd.SetBool($"equippedCharm_{charm}", false);
		pd.UnequipCharm(charm);

		return true;
	}


	public static void UpdateCharmUI() {
		if (!CharmsPaneOpen) {
			return;
		}

		PlayerData pd = Ref.PD;

		GameObject charmsPane = CharmsPane;
		GameObject equippedCharms = charmsPane.Child("Equipped Charms")!;
		GameObject textEquipped = equippedCharms.Child("Text Equipped")!;
		GameObject textOvercharmed = equippedCharms.Child("Text Overcharmed")!;
		GameObject openNotch = equippedCharms.Child("Next Dot")!;
		GameObject ocBackboard = charmsPane.Child("Overcharm", "OC Backboard")!;
		GameObject hudOCBackboard = Ref.GC.gameObject.Child("HudCamera", "Hud Canvas", "Health", "OC Backboard")!;

		PlayMakerFSM charmsFsm = charmsPane.LocateMyFSM("UI Charms");
		PlayMakerFSM overCtrlFsm = equippedCharms.Child("Notches", "Over Indicator")!
			.LocateMyFSM("Over Control");

		equippedCharms.Child("Charms")!.GetChildren().ForEach(UObject.Destroy);
		ReflectionHelper.CallMethod(equippedCharms.GetComponent<BuildEquippedCharms>(), "BuildCharmList");
		FSMUtility.SendEventToGameObject(equippedCharms, "UP");

		int overSlots = pd.GetInt(nameof(PlayerData.charmSlotsFilled)) - pd.GetInt(nameof(PlayerData.charmSlots));

		if (pd.GetBool(nameof(PlayerData.overcharmed))) {
			textOvercharmed.SetActive(true);
			textEquipped.SetActive(false);
			overCtrlFsm.FsmVariables.GetFsmInt("Cost").Value = overSlots;
			overCtrlFsm.FsmVariables.GetFsmBool("Overcharmed").Value = true;
			overCtrlFsm.SendEvent("DISPLAY OVER");
			ocBackboard.GetTransformDelegate().LocalY = -3.91f;
			hudOCBackboard.SetActive(true);
		} else {
			textOvercharmed.SetActive(false);
			textEquipped.SetActive(true);
			FSMUtility.SendEventToGameObject(textEquipped, "UP");
			overCtrlFsm.SendEvent("OVERCHARM END");
			ocBackboard.GetTransformDelegate().LocalY = -50f;
			hudOCBackboard.SetActive(false);
		}

		FSMUtility.SendEventToGameObject(charmsPane.Child("Collected Charms")!, "UPDATE", true);

		if (overSlots < 0) {
			charmsFsm.FsmVariables.GetFsmBool("Open Slot").Value = true;
			FSMUtility.SendEventToGameObject(openNotch, "NOTCH DEF UP");
		} else {
			charmsFsm.FsmVariables.GetFsmBool("Open Slot").Value = false;
			FSMUtility.SendEventToGameObject(openNotch, "NOTCH DOWN");
		}
	}
}
