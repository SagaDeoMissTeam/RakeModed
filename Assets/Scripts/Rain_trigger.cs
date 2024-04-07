// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Rain_trigger
using UnityEngine;

public class Rain_trigger : MonoBehaviour
{
	public void OnTriggerEnter(Collider info)
	{
		if (info.GetComponent<Collider>().tag == "Player")
		{
			RainManager.instance.SetPlayerRainState(state: false);
		}
	}

	public void OnTriggerExit(Collider info)
	{
		if (info.GetComponent<Collider>().tag == "Player")
		{
			RainManager.instance.SetPlayerRainState(state: true);
		}
	}
}
