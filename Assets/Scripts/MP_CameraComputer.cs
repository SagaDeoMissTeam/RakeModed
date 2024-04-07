// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_CameraComputer
using System.Collections;
using UnityEngine;

public class MP_CameraComputer : MP_DynamicObject
{
	public Animator effects;

	public bool processing;

	public bool bWatching;

	public GameObject cameraManager_UI;

	public float rotationSpeed;

	public float smoothTime;

	private Quaternion cameraTargetRot = Quaternion.identity;

	private float rotationY;

	public AudioSource audioSource;

	public AudioSource motion;

	public static MP_CameraComputer instance;

	public AudioClip startSound;

	public override void Action()
	{
		if (!processing)
		{
			if (MP_CameraManager.instance.GetCameraCount() > 0)
			{
				StartCoroutine(changeType(type: false));
			}
			else
			{
				MP_InformationUI.instance.DrawHint("You did not setup cameras");
			}
		}
	}

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
		if (bWatching)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				StartCoroutine(changeType(type: true));
			}
			motion.volume = Mathf.Abs(Input.GetAxis("Horizontal"));
			rotationY += Input.GetAxis("Horizontal") * rotationSpeed;
			rotationY = Mathf.Clamp(rotationY, -45f, 45f);
			if (MP_CameraManager.instance.currentCamera != null)
			{
				MP_CameraManager.instance.currentCamera.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, rotationY, 0f);
			}
		}
	}

	public void Exit()
	{
		StartCoroutine(changeType(type: true));
	}

	public void ResetCameraPosition()
	{
		rotationY = 0f;
		StartCoroutine(switchEffect());
	}

	private IEnumerator switchEffect()
	{
		yield return new WaitForSeconds(0.2f);
	}

	private IEnumerator changeType(bool type)
	{
		if (!type)
		{
			audioSource.PlayOneShot(startSound);
			bWatching = false;
			MP_WeaponManager.instance.GetComponent<NetworkView>().RPC("HideCurrentWeapon", RPCMode.All);
			processing = true;
			yield return new WaitForSeconds(0.1f);
			MP_InformationUI.instance.DrawItemInfo(string.Empty, showType: false);
			MP_FPSController.instance.DisablePlayer();
			MP_CameraManager.instance.ActiveCamera(0);
			yield return new WaitForSeconds(0.1f);
			cameraManager_UI.SetActive(value: true);
			bWatching = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			motion.mute = false;
			processing = false;
			yield break;
		}
		audioSource.PlayOneShot(startSound);
		bWatching = false;
		motion.mute = true;
		processing = true;
		yield return new WaitForSeconds(0.1f);
		MP_FPSController.instance.EnablePlayer();
		MP_CameraManager.instance.DisableAllCameras();
		cameraManager_UI.SetActive(value: false);
		yield return new WaitForSeconds(0.1f);
		if (RainManager.instance != null)
		{
			RainManager.instance.currentPlayer = RainManager.instance.defaultPlayer;
		}
		bWatching = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		processing = false;
	}
}
