// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// GamePauseMenu
using UnityEngine;

public class GamePauseMenu : MonoBehaviour
{
	public bool isPaused;

	public GameObject menu;

	private void Update()
	{
		if (Input.GetButtonDown("Escape"))
		{
			isPaused = !isPaused;
			if (isPaused)
			{
				menu.SetActive(value: true);
				FPSController.instance.SetPlayerDisableState("PAUSEMENU");
				Time.timeScale = 0f;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				menu.SetActive(value: false);
				FPSController.instance.SetPlayerDisableState("BEFOREPAUSE");
				Time.timeScale = 1f;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}

	public void Continue()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		menu.SetActive(value: false);
		FPSController.instance.SetPlayerDisableState("BEFOREPAUSE");
		Time.timeScale = 1f;
	}

	public void Exit()
	{
		Application.LoadLevel(0);
	}
}
