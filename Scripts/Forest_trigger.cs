// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Forest_trigger
using UnityEngine;

public class Forest_trigger : Trigger
{
	public Objective obj;

	public override void Action(Collider info)
	{
		if (info.GetComponent<Collider>().tag == "Player")
		{
			obj.ObjectiveAction();
		}
	}
}
