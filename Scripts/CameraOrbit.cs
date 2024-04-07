// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraOrbit
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("Camera-Control/CameraOrbit")]
public class CameraOrbit : MonoBehaviour
{
	public Transform target;

	public float targetHeight = 1.7f;

	public float distance = 10f;

	public float maxDistance = 20f;

	public float minDistance = 0.6f;

	public float xSpeed = 200f;

	public float ySpeed = 200f;

	public int yMinLimit = -80;

	public int yMaxLimit = 80;

	public int zoomRate = 40;

	public float rotationDampening = 3f;

	public float zoomDampening = 5f;

	private float xDeg;

	private float yDeg;

	private float currentDistance;

	private float desiredDistance;

	private float correctedDistance;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		xDeg = eulerAngles.y;
		yDeg = eulerAngles.x;
		currentDistance = distance;
		desiredDistance = distance;
		correctedDistance = distance;
	}

	private void LateUpdate()
	{
		if (!target)
		{
			return;
		}
		if (Input.GetMouseButton(0))
		{
			if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		}
		else if (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f)
		{
			float y = target.eulerAngles.y;
			float y2 = base.transform.eulerAngles.y;
			xDeg = Mathf.LerpAngle(y2, y, rotationDampening * Time.deltaTime);
		}
		yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
		Quaternion quaternion = Quaternion.Euler(yDeg, xDeg, 0f);
		desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * (float)zoomRate * Mathf.Abs(desiredDistance);
		desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
		correctedDistance = desiredDistance;
		Vector3 vector = new Vector3(0f, 0f - targetHeight, 0f);
		Vector3 vector2 = target.position - (quaternion * Vector3.forward * desiredDistance + vector);
		bool flag = false;
		currentDistance = ((flag && !(correctedDistance > currentDistance)) ? correctedDistance : Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening));
		currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
		vector2 = target.position - (quaternion * Vector3.forward * currentDistance + vector);
		base.transform.rotation = quaternion;
		base.transform.position = vector2;
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
