namespace Osmi.Game;

[PublicAPI]
public class CustomDreamnailReaction : MonoBehaviour {
	private Action<Collider2D>? method;

	public void SetMethod(Action<Collider2D> action) => method = action;

	public void SetMethod(Action<CustomDreamnailReaction, Collider2D> action) => method = action.Bind1(this);

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag != "Dream Attack") {
			return;
		}

		method?.Invoke(collision);
	}
}
