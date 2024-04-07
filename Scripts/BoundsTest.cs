// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// BoundsTest
using UnityEngine;

public class BoundsTest : MonoBehaviour
{
	public Transform pointA;

	public Transform pointB;

	public float angle;

	public float rayWidth;

	public GameObject edge;

	private RaycastHit hitBS;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			test();
		}
		angle = base.transform.rotation.x;
		rayWidth = angle * 2f;
		RaycastHit hit;
		if (Physics.Raycast(base.transform.position + base.transform.forward * 0.5f + base.transform.up * 2f, -Vector3.up, out hit))
		{
		}
		if (Physics.Raycast(base.transform.position + base.transform.forward * 0.5f + base.transform.up * 2f, -Vector3.up, out hitBS))
		{
		}
		Debug.DrawRay(base.transform.position + base.transform.forward * 0.5f + base.transform.up * 2f, -Vector3.up, Color.red);
		Debug.DrawRay(hitBS.point - new Vector3(0f, 0.1f, 0f) - base.transform.forward * 0.5f, base.transform.forward, Color.red);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.transform.position + base.transform.forward * 0.5f + base.transform.up * 2f, 0.1f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(hitBS.point - new Vector3(0f, 0.1f, 0f) - base.transform.forward * 0.5f, new Vector3(0.1f, 0.1f, 0.1f));
	}

	private void test()
	{
        RaycastHit hitInfo;
        if (!Physics.Raycast(base.transform.position + base.transform.forward * 0.5f + base.transform.up * 2f, -Vector3.up, out hitInfo))
		{
			return;
		}
		MonoBehaviour.print(hitInfo.distance);
		if (hitInfo.distance < 1f && Physics.Raycast(hitInfo.point - new Vector3(0f, 0.1f, 0f) - base.transform.forward * 0.5f, base.transform.forward, out hitInfo))
		{
			MonoBehaviour.print("ss");
			float distance = hitInfo.distance;
			if (distance > 0f && distance < 1.5f)
			{
				MonoBehaviour.print("ms");
				Vector3 position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
				GameObject gameObject = (GameObject)Object.Instantiate(pointA, position, Quaternion.identity);
				edge = gameObject;
				edge.transform.up = hitInfo.normal;
				edge.transform.forward = hitInfo.normal;
			}
		}
	}
}
