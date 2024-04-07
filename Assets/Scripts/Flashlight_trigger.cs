// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Flashlight_trigger
using UnityEngine;

public class Flashlight_trigger : Trigger
{
	public bool SetLightState;

	public override void Action(Collider info)
	{
		if (info.GetComponent<Collider>().tag == "Player")
		{
			info.GetComponent<FPSController>().SetLightState(SetLightState);
		}
	}
}
