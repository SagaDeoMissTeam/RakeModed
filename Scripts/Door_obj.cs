// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Door_obj
using System.Collections;
using UnityEngine;

public class Door_obj : DynamicObject
{
	public enum Door_state
	{
		Open,
		Close
	}

	public Door_state doorState = Door_state.Close;

	public GameObject door;

	private bool isProcessing;

	public AudioClip doorOpenSound;

	public AudioClip doorCloseSound;

	public override void Action()
	{
		if (doorState == Door_state.Close && !isProcessing)
		{
			AudioSource.PlayClipAtPoint(doorOpenSound, base.transform.position);
			door.GetComponent<Animation>().Play("Door_open");
			StartCoroutine(coroutine(Door_state.Open));
		}
		if (doorState == Door_state.Open && !isProcessing)
		{
			AudioSource.PlayClipAtPoint(doorCloseSound, base.transform.position);
			door.GetComponent<Animation>().Play("Door_close");
			StartCoroutine(coroutine(Door_state.Close));
		}
	}

	private IEnumerator coroutine(Door_state state)
	{
		isProcessing = true;
		yield return new WaitForSeconds(0.5f);
		doorState = state;
		if (state == Door_state.Open)
		{
			name = "Press \"E\" to close door";
		}
		else
		{
			name = "Press \"E\" to open door";
		}
		yield return new WaitForSeconds(0.5f);
		isProcessing = false;
	}
}
