// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Camera_weapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_weapon : Weapon
{
	public Animator animator;

	public Animator UI;

	public GameObject UI_System;

	public bool bSetup;

	public Text ammoDisplay;

	private void OnEnable()
	{
		UI_System.SetActive(value: true);
		bSetup = false;
	}

	private void OnDisable()
	{
		if ((bool)UI_System)
		{
			UI_System.SetActive(value: false);
		}
	}

	private void Update()
	{
		ammo = WeaponManager.instance.weaponsAmmo[weaponID];
		ammoDisplay.text = ammo.ToString();
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 2.5f) && (bool)hitInfo.collider.GetComponent<RECCameras_obj>())
		{
			RECCameras_obj component = hitInfo.collider.GetComponent<RECCameras_obj>();
			if (Input.GetButtonDown("Use") && WeaponManager.instance.weaponsAmmo[weaponID] > 0 && !bSetup)
			{
				StartCoroutine(Setup(component));
			}
		}
	}

	private IEnumerator Setup(RECCameras_obj cam)
	{
		if (cam.CheckSetup())
		{
			bSetup = true;
			List<int> weaponsAmmo;
			List<int> list = (weaponsAmmo = WeaponManager.instance.weaponsAmmo);
			int index;
			int index2 = (index = weaponID);
			index = weaponsAmmo[index];
			list[index2] = index - 1;
			animator.SetBool("Setup", value: true);
			yield return new WaitForSeconds(0.1f);
			cam.SetupCamera();
			yield return new WaitForSeconds(0.2f);
			bSetup = false;
			yield return new WaitForSeconds(0.5f);
			if (WeaponManager.instance.weaponsAmmo[weaponID] > 0)
			{
				animator.SetBool("Setup", value: false);
				yield break;
			}
			WeaponManager.instance.RemoveWeapon(weaponID);
			UI_System.SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
	}
}
