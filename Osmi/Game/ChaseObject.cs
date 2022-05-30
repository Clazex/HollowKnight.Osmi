namespace Osmi.Game;

[PublicAPI]
[RequireComponent(typeof(Rigidbody2D))]
public class ChaseObject : MoveToPosition2D {
	public override Vector2? TargetPos {
		get => Target?.position.AsVector2();
		set {
			if (value.HasValue) {
				Target?.SetPosition2D(value.Value.x, value.Value.y);
			}
		}
	}

	public Transform? Target { get; set; } = null;
}
