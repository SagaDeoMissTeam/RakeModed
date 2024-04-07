// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CameraManager
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
	public static CameraManager instance;

	public int currentCameraID;

	public CameraComputer cameraComp;

	public AudioClip cameraSelect;

	public AudioSource audioSource;

	public Transform currentCamera;

	private int CameraCount;

	public List<Transform> gameCameras = new List<Transform>();

	public List<Transform> allCameras = new List<Transform>();

	public List<Button> cameraButtons = new List<Button>();

	public int cameraCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < gameCameras.Count; i++)
			{
				if (gameCameras[i] != null)
				{
					num++;
				}
			}
			return num;
		}
	}

	public void Start()
	{
		instance = this;
	}

	private void Update()
	{
	}

	public int GetCameraCount()
	{
		return gameCameras.Count;
	}

	public Transform GetCurrentCamera()
	{
		return gameCameras[currentCameraID];
	}

	public void AddCamera(Transform camera)
	{
		gameCameras.Add(camera);
		RefreshButtons();
	}

	public int FindCameraID(Transform camera)
	{
		int result = 0;
		for (int i = 0; i < gameCameras.Count; i++)
		{
			if (gameCameras[i] == camera)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public void RemoveCamera(Transform camera)
	{
		gameCameras.Remove(camera);
		RefreshButtons();
		if (cameraComp.bWatching)
		{
			if (gameCameras.Count > 0)
			{
				ActiveCamera(0);
			}
			else
			{
				cameraComp.Exit();
			}
		}
	}

	private void RefreshButtons()
	{
		for (int i = 0; i < cameraButtons.Count; i++)
		{
			if (i < gameCameras.Count)
			{
				cameraButtons[i].interactable = true;
			}
			else
			{
				cameraButtons[i].interactable = false;
			}
		}
	}

	public void Event_activeCamera(int camID)
	{
		ColorBlock colors = cameraButtons[camID].colors;
		colors.normalColor = Color.red;
		cameraButtons[camID].colors = colors;
	}

	public void Event_disableCamera(int camID)
	{
		ColorBlock colors = cameraButtons[camID].colors;
		colors.normalColor = Color.white;
		cameraButtons[camID].colors = colors;
	}

	public void DisableAllCameras()
	{
		for (int i = 0; i < gameCameras.Count; i++)
		{
			gameCameras[i].GetChild(0).gameObject.SetActive(value: false);
		}
	}

	public void ActiveCamera(int id)
	{
		for (int i = 0; i < gameCameras.Count; i++)
		{
			if (i == id && gameCameras[i] != null)
			{
				gameCameras[i].GetChild(0).gameObject.SetActive(value: true);
				currentCameraID = id;
				currentCamera = gameCameras[i].GetChild(0);
				cameraComp.ResetCameraPosition();
				gameCameras[i].parent.GetComponent<RecordCamera_obj>().isRecord = true;
				audioSource.PlayOneShot(cameraSelect);
				if (RainManager.instance != null)
				{
					RainManager.instance.currentPlayer = currentCamera;
				}
			}
			else if (gameCameras[i] != null)
			{
				gameCameras[i].GetChild(0).gameObject.SetActive(value: false);
			}
		}
		Debug.Log(id);
	}

	public void Shot()
	{
		NotificationsManager.Instance.PostNotification(this, "Event_PlayerFire");
	}
}
