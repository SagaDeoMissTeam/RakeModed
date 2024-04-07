// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_TestAI
using System.Collections;
using UnityEngine;

public class MP_TestAI : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(AI_Patrol());
	}

	private IEnumerator AI_Patrol()
	{
		Debug.Log("start");
		while (true)
		{
			yield return null;
			if (Input.GetKeyDown(KeyCode.A))
			{
				break;
			}
			Debug.Log(Time.deltaTime);
		}
	}
}
