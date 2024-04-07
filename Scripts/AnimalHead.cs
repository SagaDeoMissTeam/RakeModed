// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AnimalHead
using UnityEngine;

public class AnimalHead : MonoBehaviour
{
	public Transform target;

	public float fieldOfViewAngle;

	public Animal_AI animal_AI;

	public bool bSeen()
	{
		target = animal_AI.enemy;
		if (target != null)
		{
			Vector3 from = target.transform.position - base.transform.position;
			Vector3 forward = base.transform.forward;
			float num = Vector3.Angle(from, forward);
			if (num < fieldOfViewAngle)
			{
				Debug.Log("See");
				return true;
			}
			Debug.Log("Unseen");
			return false;
		}
		return false;
	}
}
