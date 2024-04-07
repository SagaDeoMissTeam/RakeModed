// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// FootstepChanger
using UnityEngine;

public class FootstepChanger : MonoBehaviour
{
	public int stepID;

	private void OnTriggerEnter(Collider player)
	{
		if (player.tag == "Player")
		{
			player.GetComponent<PlayerMovement>().SetFootstepSound(stepID);
		}
	}

	private void OnTriggerExit(Collider player)
	{
		if (player.tag == "Player")
		{
			player.GetComponent<PlayerMovement>().SetFootstepSound(0);
		}
	}
}
