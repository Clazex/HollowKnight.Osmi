namespace Osmi.Game;

[PublicAPI]
public class CustomHitEffectReceiver : MonoBehaviour, IHitEffectReciever {
	private Action<float>? method;

	public void SetMethod(Action<float> action) => method = action;

	public void SetMethod(Action<CustomHitEffectReceiver, float> action) => method = action.Bind1(this);

	public void RecieveHitEffect(float attackDirection) =>
		method?.Invoke(attackDirection);
}
