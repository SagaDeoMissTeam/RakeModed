// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// IORotator
using UnityEngine;

public class IORotator : MonoBehaviour
{
	public Vector3 rot;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(rot.x * Time.deltaTime, rot.y * Time.deltaTime, 0f);
	}
}
