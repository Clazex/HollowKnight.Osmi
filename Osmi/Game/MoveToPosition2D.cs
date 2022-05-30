namespace Osmi.Game;

[PublicAPI]
[RequireComponent(typeof(Rigidbody2D))]
public class MoveToPosition2D : MonoBehaviour {
	private Rigidbody2D rb = null!;

	public virtual Vector2? TargetPos { get; set; } = null;

	public float accelerationForce = 0;
	public float maxVelocity = 0;

	private void Start() => rb = GetComponent<Rigidbody2D>();

	private void FixedUpdate() {
		if (TargetPos == null || rb.bodyType != RigidbodyType2D.Dynamic) {
			return;
		}

		rb.AddForce((TargetPos.Value - transform.position.AsVector2()).GetUnitVector() * accelerationForce);
		rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
	}
}
