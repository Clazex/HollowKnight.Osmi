namespace Osmi.Game;

[PublicAPI]
public static class AudioUtil {
	public static void PlayOneShot(GameObject playerPrefab, Vector3 position, AudioClip clip, float pitchMin, float pitchMax, float volumn = 1f) =>
		PlayOneShot(playerPrefab, position, clip, UnityEngine.Random.Range(pitchMin, pitchMax), volumn);

	public static void PlayOneShot(GameObject playerPrefab, Vector3 position, AudioClip clip, float pitch, float volumn = 1f) {
		AudioSource source = playerPrefab.Spawn(position, Quaternion.Euler(Vector3.up)).GetComponent<AudioSource>();
		source.pitch = pitch;
		source.volume = volumn;
		source.PlayOneShot(clip);
	}
}
