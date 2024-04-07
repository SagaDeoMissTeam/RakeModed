// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// RakeDead_obj
using System.Collections;
using UnityEngine;

public class RakeDead_obj : MonoBehaviour
{
	public AudioClip sound;

	private void Start()
	{
	}

	private void OnTriggerEnter(Collider info)
	{
		if (info.GetComponent<Collider>().tag == "Player")
		{
			StartCoroutine(finishGame());
		}
	}

	private IEnumerator finishGame()
	{
		FPSController.instance.effectAnimator.SetBool("Fade", value: true);
		yield return new WaitForSeconds(2f);
		AudioSource.PlayClipAtPoint(sound, base.transform.position);
		InformationUI.instance.DrawObjective("You win?");
		yield return new WaitForSeconds(2f);
		Application.LoadLevel(0);
	}
}
