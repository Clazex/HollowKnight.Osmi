namespace Osmi.Game;

[PublicAPI]
public class CustomBehaviour : MonoBehaviour {
	public event Action<CustomBehaviour>? StartEvent;
	public event Action<CustomBehaviour>? OnEnableEvent;
	public event Action<CustomBehaviour>? OnDisableEvent;
	public event Action<CustomBehaviour>? OnDestroyEvent;
	public event Action<CustomBehaviour>? UpdateEvent;
	public event Action<CustomBehaviour>? FixedUpdateEvent;

	private void Start() => StartEvent?.Invoke(this);
	private void OnEnable() => OnEnableEvent?.Invoke(this);
	private void OnDisable() => OnDisableEvent?.Invoke(this);
	private void OnDestroy() => OnDestroyEvent?.Invoke(this);
	private void Update() => UpdateEvent?.Invoke(this);
	private void FixedUpdate() => FixedUpdateEvent?.Invoke(this);
}
