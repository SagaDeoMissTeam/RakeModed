// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// LevelSkip
using System.Collections;
using UnityEngine;

public class LevelSkip : MonoBehaviour
{
	public GameObject HUD;

	public int levelToLoad;

	public CameraMouseLook cml;

	private void OnTriggerEnter(Collider info)
	{
		if (info.GetComponent<Collider>().tag == "Player")
		{
			cml = info.GetComponent<CameraMouseLook>();
			cml.bControl = false;
			HUD.SetActive(value: true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			WeaponManager.instance.HideCurrentWeapon();
		}
	}

	private void OnTriggerExit(Collider info)
	{
		if (info.GetComponent<Collider>().tag == "Player")
		{
			Exit();
		}
	}

	public void Load()
	{
	}

	public void Exit()
	{
		cml.bControl = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		HUD.SetActive(value: false);
	}

	private IEnumerator wait()
	{
		SaveGameSystem.instance.SaveGame();
		FPSController.instance.effectAnimator.SetBool("Fade", value: true);
		HUD.SetActive(value: false);
		yield return new WaitForSeconds(1f);
		Application.LoadLevel(levelToLoad);
	}

	public void SkipLevel(bool state)
	{
		if (state)
		{
			StartCoroutine(wait());
		}
		else
		{
			Exit();
		}
	}
}
