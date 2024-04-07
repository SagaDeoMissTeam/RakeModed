// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// TrapRotationCorrect
using UnityEngine;

public class TrapRotationCorrect : MonoBehaviour
{
	private void Update()
	{
		Ray ray = new Ray(base.transform.position, Vector3.down);
		RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 5f) && (bool)hitInfo.collider.GetComponent<Terrain>())
		{
			base.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
		}
	}
}
