// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_RecordCamera_obj
using UnityEngine;
using UnityEngine.UI;

public class MP_RecordCamera_obj : MP_RECCamera_obj
{
	public enum Camera_state
	{
		Unsetup,
		Setup
	}

	public bool isRecord;

	public Text text;

	public GameObject setupCamera;

	public GameObject cameraPoint;

	public GameObject cameraRender;

	public Camera_state cameraState;

	public Transform backlight;

	public Transform currentTarget;

	public Transform camRec;

	public AudioClip setupSound;

	public int cameraID;

	private void Start()
	{
		name = "Press 'E' to setup camera";
	}

	private void AddCameraItem()
	{
		if (MP_WeaponManager.instance.CheckWeapon(2))
		{
			if (!MP_WeaponManager.instance.bFullAmmo(2))
			{
				MP_WeaponManager.instance.AddAmmo(2, 1);
				GetComponent<NetworkView>().RPC("SetUnsetupState", RPCMode.All);
			}
		}
		else
		{
			MP_WeaponManager.instance.AddWeapon(2, 1, type: true);
			GetComponent<NetworkView>().RPC("SetUnsetupState", RPCMode.All);
		}
	}

	[RPC]
	public void SetSetupState()
	{
		cameraState = Camera_state.Setup;
		setupCamera.SetActive(value: true);
		cameraPoint.SetActive(value: false);
		cameraRender.SetActive(value: false);
		MP_CameraManager.instance.AddCamera(setupCamera.transform);
		AudioSource.PlayClipAtPoint(setupSound, base.transform.position);
		name = "Press 'E' to pickup camera ";
	}

	[RPC]
	public void SetUnsetupState()
	{
		cameraState = Camera_state.Unsetup;
		setupCamera.SetActive(value: false);
		cameraPoint.SetActive(value: true);
		cameraRender.SetActive(value: false);
		MP_CameraManager.instance.RemoveCamera(setupCamera.transform);
		AudioSource.PlayClipAtPoint(setupSound, base.transform.position);
		name = "Press 'E' to setup camera ";
	}

	[RPC]
	public override void SetupCamera()
	{
		if (cameraState == Camera_state.Unsetup)
		{
			GetComponent<NetworkView>().RPC("SetSetupState", RPCMode.All);
		}
	}

	public override void Action()
	{
		if (cameraState == Camera_state.Unsetup && MP_WeaponManager.instance.CurrentWeapon() != 2)
		{
			if (MP_WeaponManager.instance.CheckWeapon(2))
			{
				MP_InformationUI.instance.DrawHint("Please select camera");
			}
			else
			{
				MP_InformationUI.instance.DrawHint("You do not have camera");
			}
		}
		if (cameraState == Camera_state.Setup && MP_WeaponManager.instance.CurrentWeapon() != 2)
		{
			Debug.Log("addCAmeraitem");
			AddCameraItem();
		}
	}

	private void Update()
	{
		if (cameraState == Camera_state.Unsetup)
		{
			UpdateColor();
		}
		if (cameraState == Camera_state.Setup && !(currentTarget != null))
		{
		}
	}

	private void UpdateColor()
	{
		Renderer component = backlight.GetComponent<Renderer>();
		Color color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, Mathf.PingPong(Time.time * 0.1f, 0.1f));
		component.material.SetColor("_TintColor", color);
	}

	public override bool CheckSetup()
	{
		if (cameraState == Camera_state.Unsetup)
		{
			if (MP_CameraManager.instance.cameraCount >= 5)
			{
				MP_InformationUI.instance.DrawHint("You cannot setup no more 5 cameras");
				return false;
			}
			return true;
		}
		AddCameraItem();
		return false;
	}
}
