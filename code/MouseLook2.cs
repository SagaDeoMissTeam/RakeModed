// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MouseLook2
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("Camera-Control/Mouse Look2")]
public class MouseLook2 : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}

	public RotationAxes axes;

	public float sensitivityX = 5f;

	public float sensitivityY = 5f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -60f;

	public float maximumY = 60f;

	private float rotationY;

	private void Update()
	{
		if (Input.GetMouseButton(0) && (!(EventSystem.current != null) || !EventSystem.current.IsPointerOverGameObject()))
		{
			if (axes == RotationAxes.MouseXAndY)
			{
				float y = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				base.transform.localEulerAngles = new Vector3(0f - rotationY, y, 0f);
			}
			else if (axes == RotationAxes.MouseX)
			{
				base.transform.Rotate(0f, Input.GetAxis("Mouse X") * sensitivityX, 0f);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				base.transform.localEulerAngles = new Vector3(0f - rotationY, base.transform.localEulerAngles.y, 0f);
			}
		}
	}

	private void Start()
	{
		if ((bool)GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}
}
