namespace Osmi.Game;

#pragma warning disable IDE0051

[PublicAPI]
[RequireComponent(typeof(Rigidbody2D))]
public class MoveToPosition2D : MonoBehaviour {
	private Rigidbody2D rb = null!;

	public virtual Vector2? TargetPos { get; set; } = null;

	public float accelerationForce = 0;
	public float maxVelocity = 0;


	private void Start() => rb = GetComponent<Rigidbody2D>();

	private void FixedUpdate() {
		if (!TargetPos.HasValue || rb.bodyType != RigidbodyType2D.Dynamic) {
			return;
		}

		Vector2 relPos = TargetPos.Value - transform.position.AsVector2();
		if (relPos.magnitude == 0) {
			return;
		}

		rb.AddForce(relPos.GetUnitVector() * accelerationForce);
		rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
	}
}
