// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Trigger
using UnityEngine;

public class Trigger : MonoBehaviour
{
	public void OnTriggerEnter(Collider info)
	{
		Action(info);
	}

	public virtual void Action(Collider info)
	{
	}
}
