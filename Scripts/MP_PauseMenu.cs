// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_PauseMenu
using UnityEngine;

public class MP_PauseMenu : MonoBehaviour
{
	public bool isPaused;

	public GameObject menu;

	public MP_PlayerController PC;

	private void Update()
	{
		if (Input.GetButtonDown("Escape") && (bool)GetComponent<NetworkView>())
		{
			if (PC == null)
			{
				PC = MP_WeaponManager.instance.GetComponent<MP_FPSController>();
			}
			isPaused = !isPaused;
			if (isPaused)
			{
				menu.SetActive(value: true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				PC.GetComponent<NetworkView>().RPC("HideCurrentWeapon", RPCMode.All);
				return;
			}
			menu.SetActive(value: false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			PC.GetComponent<NetworkView>().RPC("Camera_state", RPCMode.All, true);
			PC.GetComponent<NetworkView>().RPC("FreezePlayer", RPCMode.All, true);
			PC.GetComponent<NetworkView>().RPC("HideCurrentWeapon", RPCMode.All);
		}
	}

	public void Continue()
	{
		PC.GetComponent<NetworkView>().RPC("Camera_state", RPCMode.All, true);
		PC.GetComponent<NetworkView>().RPC("FreezePlayer", RPCMode.All, true);
		PC.GetComponent<NetworkView>().RPC("HideCurrentWeapon", RPCMode.All);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		menu.SetActive(value: false);
	}

	public void Exit()
	{
		Network.Disconnect();
		Application.LoadLevel(0);
	}
}
