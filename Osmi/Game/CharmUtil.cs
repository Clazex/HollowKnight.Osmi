namespace Osmi.Game;

[PublicAPI]
public static class CharmUtil {
	/// <summary>
	/// Gets reference to the Charms UI GameObject
	/// </summary>
	public static GameObject CharmsPane => Ref.GC.gameObject.Child("HudCamera", "Inventory", "Charms")!;

	/// <summary>
	/// Gets whether the Charms UI is opened
	/// </summary>
	public static bool CharmsPaneOpen => CharmsPane.Child("Cursor", "Back")!.activeInHierarchy;


	/// <summary>
	/// Applies the changes to the charms. Also cause the player to restore to max health.
	/// Calls <see cref="UpdateCharmUI"/> internally.
	/// </summary>
	public static void UpdateCharm() {
		Ref.HC.CharmUpdate();
		PlayMakerFSM.BroadcastEvent("CHARM INDICATOR CHECK");
		PlayMakerFSM.BroadcastEvent("UPDATE BLUE HEALTH");
		UpdateCharmUI();
	}


	/// <summary>
	/// Gets whether the player has got specified charm.
	/// </summary>
	/// <param name="charm">Charm ID</param>
	/// <returns>Whether the player has got the charm</returns>
	public static bool GotCharm(int charm) =>
		Ref.PD.GetBool($"gotCharm_{charm}");

	/// <summary>
	/// Gets whether the player has equipped specified charm.
	/// </summary>
	/// <param name="charm">Charm ID</param>
	/// <returns>Whether the player has equipped the charm</returns>
	public static bool EquippedCharm(int charm) =>
		Ref.PD.GetBool($"equippedCharm_{charm}");

	/// <summary>
	/// Gets the notch cost of specified charm.
	/// </summary>
	/// <param name="charm">Charm ID</param>
	/// <returns>Notch cost of specified charm</returns>
	public static int GetCharmCost(int charm) =>
		Ref.PD.GetInt($"charmCost_{charm}");

	/// <summary>
	/// Equips specified charm for the player.
	/// The operation fails if the charm is already equipped,
	/// or the player has already been overcharming.
	/// </summary>
	/// <param name="charm">Charm ID</param>
	/// <returns>Whether the operation is successful</returns>
	/// <remarks>The caller should call <see cref="UpdateCharm"/> after changes to charms</remarks>
	public static bool EquipCharm(int charm) => EquipCharmInternal(Ref.PD, charm);

	/// <summary>
	/// Unequips specified charm for the player.
	/// The operation fails if the charm is not equipped.
	/// </summary>
	/// <param name="charm">Charm ID</param>
	/// <returns>Whether the operation is successful</returns>
	/// <remarks>The caller should call <see cref="UpdateCharm"/> after changes to charms</remarks>
	public static bool UnequipCharm(int charm) => UnequipCharmInternal(Ref.PD, charm);

	/// <summary>
	/// Equips a series of charms for the player.
	/// </summary>
	/// <param name="charms">Charm IDs</param>
	/// <returns>Whether all operations are successful</returns>
	/// <remarks>The caller should call <see cref="UpdateCharm"/> after changes to charms</remarks>
	public static bool EquipCharms(params int[] charms) {
		PlayerData pd = Ref.PD;
		return charms.Every(charm => EquipCharmInternal(pd, charm));
	}

	/// <summary>
	/// Unequips a series of charms for the player.
	/// </summary>
	/// <param name="charms">Charm IDs</param>
	/// <returns>Whether all operations are successful</returns>
	/// <remarks>The caller should call <see cref="UpdateCharm"/> after changes to charms</remarks>
	public static bool UnequipCharms(params int[] charms) {
		PlayerData pd = Ref.PD;
		return charms.Every(charm => UnequipCharmInternal(pd, charm));
	}

	/// <summary>
	/// Unequipps all charms the player are equipping.
	/// </summary>
	/// <remarks>The caller should call <see cref="UpdateCharm"/> after changes to charms</remarks>
	public static void UnequipAllCharms() =>
		UnequipCharms(Ref.PD.GetVariable<List<int>>(nameof(PlayerData.equippedCharms)).ToArray());



	/// <inheritdoc cref="GotCharm(int)"/>
	public static bool GotCharm(Charm charm) => GotCharm((int) charm);

	/// <inheritdoc cref="EquippedCharm(int)"/>
	public static bool EquippedCharm(Charm charm) => EquippedCharm((int) charm);

	/// <inheritdoc cref="GetCharmCost(int)"/>
	public static int GetCharmCost(Charm charm) => GetCharmCost((int) charm);

	/// <inheritdoc cref="EquipCharm(int)"/>
	public static bool EquipCharm(Charm charm) => EquipCharm((int) charm);

	/// <inheritdoc cref="UnequipCharm(int)"/>
	public static bool UnequipCharm(Charm charm) => UnequipCharm((int) charm);

	/// <inheritdoc cref="EquipCharms(int[])"/>
	public static bool EquipCharms(params Charm[] charms) => EquipCharms(charms.Map(i => (int) i).ToArray());

	/// <inheritdoc cref="UnequipCharms(int[])"/>
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



	/// <summary>
	/// Updates Charms related UI to proper state.
	/// Called by <see cref="UpdateCharm"/> internally.
	/// </summary>
	public static void UpdateCharmUI() {
		if (!CharmsPaneOpen) {
			Ref.GC.gameObject
				.Child("HudCamera", "Hud Canvas", "Health", "OC Backboard")!
				.SetActive(Ref.PD.GetBool(nameof(PlayerData.overcharmed)));
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
