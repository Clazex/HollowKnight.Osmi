namespace Osmi.Utils;

[PublicAPI]
public sealed class TransformDelegate {
	public Transform Raw { get; private init; }

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
