// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_CameraMouseLook
using System;
using UnityEngine;

public class MP_CameraMouseLook : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}

	public RotationAxes axes;

	public float sensitivityX = 15f;

	public float sensitivityY = 15f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float smoothTime = 5f;

	public float minimumY = -60f;

	public float maximumY = 60f;

	private float rotationY;

	public Transform camera;

	public Transform character;

	public Quaternion m_CharacterTargetRot;

	public Quaternion m_CameraTargetRot;

	public float yRot;

	public float xRot;

	public bool bControl;

	[RPC]
	public void Camera_state(bool type)
	{
		bControl = type;
	}

	private void Start()
	{
		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = camera.localRotation;
	}

	private void Update()
	{
		if (bControl)
		{
			yRot = Input.GetAxis("Mouse X") * sensitivityX;
			xRot = Input.GetAxis("Mouse Y") * sensitivityY;
			m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);
			m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
			m_CameraTargetRot *= Quaternion.Euler(0f - xRot, 0f, 0f);
			character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot, smoothTime * Time.deltaTime);
			camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot, smoothTime * Time.deltaTime);
		}
	}

	[RPC]
	public void Focus(Vector3 target)
	{
		Vector3 forward = target - base.transform.position;
		Quaternion rotation = Quaternion.LookRotation(forward);
		base.transform.rotation = rotation;
	}

	private Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1f;
		float value = 114.59156f * Mathf.Atan(q.x);
		value = Mathf.Clamp(value, minimumY, maximumY);
		q.x = Mathf.Tan((float)Math.PI / 360f * value);
		return q;
	}
}
