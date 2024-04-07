// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Log_trigger
using UnityEngine;

public class Log_trigger : Trigger
{
	public string text;

	public override void Action(Collider info)
	{
		if (info.gameObject.tag == "Player")
		{
			InformationUI.instance.DrawHint(text);
			Object.Destroy(base.gameObject);
		}
	}
}
