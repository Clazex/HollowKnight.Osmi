namespace Osmi.Game;

[PublicAPI]
[RequireComponent(typeof(Rigidbody2D))]
public class ChaseObject : MoveToPosition2D {
	public override Vector2? TargetPos {
		get => isNull ? null : Target!.position.AsVector2();
		set {
			if (value.HasValue && !isNull) {
				Target!.SetPosition2D(value.Value.x, value.Value.y);
			}
		}
	}

	private bool isNull = true;
	private Transform? tf = null!;

	public Transform? Target {
		get => tf;
		set {
			isNull = value == null;
			tf = value;
		}
	}
}
