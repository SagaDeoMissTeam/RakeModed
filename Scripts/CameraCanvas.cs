// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraCanvas
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraCanvas : MonoBehaviour
{
	public List<Image> points = new List<Image>();

	private void OnEnable()
	{
		GetCamerasState();
	}

	private void GetCamerasState()
	{
		for (int i = 0; i < CameraManager.instance.allCameras.Count; i++)
		{
			if (CameraManager.instance.allCameras[i].GetComponent<RecordCamera_obj>().cameraState == RecordCamera_obj.Camera_state.Setup)
			{
				points[i].color = Color.green;
			}
			else
			{
				points[i].color = Color.red;
			}
		}
	}
}
