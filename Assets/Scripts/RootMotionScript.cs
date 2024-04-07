// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// RootMotionScript
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootMotionScript : MonoBehaviour
{
	private void OnAnimatorMove()
	{
		Animator component = GetComponent<Animator>();
		if ((bool)component)
		{
			Vector3 position = base.transform.position;
			position.z += component.GetFloat("Runspeed") * Time.deltaTime;
			base.transform.position = position;
		}
	}
}
