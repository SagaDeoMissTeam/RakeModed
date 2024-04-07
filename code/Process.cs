// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Process
using System.Collections;
using UnityEngine;

public class Process : MonoBehaviour
{
	private IEnumerator coroutine;

	private void Start()
	{
		MonoBehaviour.print("Starting " + Time.time);
		coroutine = WaitAndPrint(3f);
		StartCoroutine(coroutine);
		MonoBehaviour.print("Done " + Time.time);
	}

	public IEnumerator WaitAndPrint(float waitTime)
	{
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			MonoBehaviour.print("WaitAndPrint " + Time.time);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			StopCoroutine(coroutine);
			MonoBehaviour.print("Stopped " + Time.time);
		}
	}
}
