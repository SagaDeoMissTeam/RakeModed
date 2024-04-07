// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DetectionTriggerAnimal
using UnityEngine;

public class DetectionTriggerAnimal : MonoBehaviour
{
	public Animal_AI ai;

	private void OnTriggerEnter(Collider info)
	{
		if (info.gameObject.tag == "Player")
		{
			ai.SetDangerState();
		}
		if (info.gameObject.tag == "Rake")
		{
			ai.bEnemyInRadius = true;
			ai.enemy = info.gameObject.transform;
		}
	}

	private void OnTriggerStay(Collider info)
	{
		if (info.gameObject.tag == "Player")
		{
			ai.bEnemyInRadius = true;
		}
	}

	private void OnTriggerExit(Collider info)
	{
		if (info.gameObject.tag == "Player")
		{
			ai.bEnemyInRadius = false;
		}
	}
}
