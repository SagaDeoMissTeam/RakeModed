// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraAngle
using UnityEngine;

public class CameraAngle : MonoBehaviour
{
	public Transform camera;

	public Transform target;

	private bool isView;

	public Transform cam;

	public int id;

	public void OnTriggerEnter(Collider info)
	{
		if ((bool)info.gameObject.GetComponent<Unit_obj>())
		{
			target = info.gameObject.transform;
		}
	}

	public void OnTriggerExit(Collider info)
	{
		if ((bool)info.gameObject.GetComponent<Unit_obj>() && target == info.gameObject.transform)
		{
			isView = false;
			target = null;
			CameraManager.instance.Event_disableCamera(id);
		}
	}

	private void Update()
	{
		if (target != null)
		{
			Vector3 to = target.transform.position - base.transform.position;
			if (Vector3.Angle(camera.transform.position, to) < 60f && !isView)
			{
				isView = true;
				id = CameraManager.instance.FindCameraID(cam);
				CameraManager.instance.Event_activeCamera(id);
			}
		}
	}
}
