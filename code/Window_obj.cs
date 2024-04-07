// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Window_obj
using UnityEngine;

public class Window_obj : MonoBehaviour
{
	public GameObject glass;

	public GameObject brokenGlass;

	public GameObject particle;

	public AudioClip sound;

	public int bDestroy;

	public void SetBrokenState()
	{
		glass.SetActive(value: false);
		brokenGlass.SetActive(value: true);
		bDestroy = 1;
		GetComponent<BoxCollider>().enabled = false;
	}

	public void BreakGlass()
	{
		glass.SetActive(value: false);
		brokenGlass.SetActive(value: true);
		particle.SetActive(value: true);
		AudioSource.PlayClipAtPoint(sound, base.transform.position);
		GetComponent<BoxCollider>().enabled = false;
	}

	public void TakeDamage(int amount, string damageType)
	{
		if (bDestroy == 0)
		{
			BreakGlass();
			bDestroy = 1;
		}
	}
}
