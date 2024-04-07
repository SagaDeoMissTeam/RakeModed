// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AutoDestroy
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	public float TD;

	private float timer;

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > TD)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
