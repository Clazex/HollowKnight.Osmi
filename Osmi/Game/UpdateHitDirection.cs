namespace Osmi.Game;

[PublicAPI]
[RequireComponent(typeof(DamageEnemies))]
public sealed class UpdateHitDirection : MonoBehaviour {
	private DamageEnemies de = null!;

	public void Start() =>
		de = GetComponent<DamageEnemies>();

	public void FixedUpdate() =>
		de.direction = gameObject.transform.rotation.z;
}
