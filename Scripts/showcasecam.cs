// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// showcasecam
using UnityEngine;

public class showcasecam : MonoBehaviour
{
	public Transform rotationPoint;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.RotateAround(rotationPoint.position, Vector3.up, 20f * Time.deltaTime);
	}
}
