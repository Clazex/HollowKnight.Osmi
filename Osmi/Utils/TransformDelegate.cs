namespace Osmi.Utils;

[PublicAPI]
public sealed class TransformDelegate {
	public Transform Raw { get; private init; }

	public Vector2 Position2D {
		get => Raw.position.AsVector2();
		set => Raw.position = value.ToVector3(Raw.position.z);
	}

	public float X {
		get => Raw.position.x;
		set => Raw.position = Raw.position with { x = value };
	}

	public float Y {
		get => Raw.position.y;
		set => Raw.position = Raw.position with { y = value };
	}

	public float Z {
		get => Raw.position.z;
		set => Raw.position = Raw.position with { z = value };
	}


	public Vector2 LocalPosition2D {
		get => Raw.localPosition.AsVector2();
		set => Raw.localPosition = value.ToVector3(Raw.localPosition.z);
	}

	public float LocalX {
		get => Raw.localPosition.x;
		set => Raw.localPosition = Raw.localPosition with { x = value };
	}

	public float LocalY {
		get => Raw.localPosition.y;
		set => Raw.localPosition = Raw.localPosition with { y = value };
	}

	public float LocalZ {
		get => Raw.localPosition.z;
		set => Raw.localPosition = Raw.localPosition with { z = value };
	}


	public Vector2 Scale2D {
		get => Raw.localScale.AsVector2();
		set => Raw.localScale = value.ToVector3(Raw.localScale.z);
	}

	public float ScaleX {
		get => Raw.localScale.x;
		set => Raw.localScale = Raw.localScale with { x = value };
	}

	public float ScaleY {
		get => Raw.localScale.y;
		set => Raw.localScale = Raw.localScale with { y = value };
	}

	public float ScaleZ {
		get => Raw.localScale.z;
		set => Raw.localScale = Raw.localScale with { z = value };
	}


	public float Rotation2D {
		get => Raw.localEulerAngles.z;
		set => Raw.localEulerAngles = Raw.localEulerAngles with { z = value };
	}


	public TransformDelegate(Transform tf) => Raw = tf;
}
