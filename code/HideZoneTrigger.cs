// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// HideZoneTrigger
using UnityEngine;

public class HideZoneTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider info)
	{
		if (info.gameObject.tag == "Player")
		{
			info.gameObject.GetComponent<FPSController>().bHide = true;
		}
	}

	private void OnTriggerExit(Collider info)
	{
		if (info.gameObject.tag == "Player")
		{
			info.gameObject.GetComponent<FPSController>().bHide = false;
		}
	}
}
