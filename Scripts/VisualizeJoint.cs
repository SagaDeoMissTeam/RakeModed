// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// VisualizeJoint
using UnityEngine;

[ExecuteInEditMode]
public class VisualizeJoint : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(base.transform.position, 0.015f);
		foreach (Transform item in base.transform)
		{
			Gizmos.DrawLine(base.transform.position, item.position);
		}
	}
}
