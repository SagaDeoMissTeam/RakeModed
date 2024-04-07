// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SoundTrigger
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
	public AudioClip sound;

	public int id;

	private void OnTriggerEnter(Collider info)
	{
		if (info.gameObject.tag == "Player")
		{
			AudioSource.PlayClipAtPoint(sound, base.transform.position, 1f);
			Object.Destroy(base.gameObject);
		}
	}
}
