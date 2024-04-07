// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MedKit_weapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedKit_weapon : Weapon
{
	public Animator animator;

	public AudioSource audioSource;

	public AudioClip useSound;

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
		ammoDisplay.text = ammo.ToString();
		ammo = WeaponManager.instance.weaponsAmmo[weaponID];
		if (Input.GetButtonDown("Fire1") && WeaponManager.instance.weaponsAmmo[weaponID] > 0 && !bSetup)
		{
			StartCoroutine(Use());
		}
	}

	private IEnumerator Use()
	{
		bSetup = true;
		List<int> weaponsAmmo;
		List<int> list = (weaponsAmmo = WeaponManager.instance.weaponsAmmo);
		int index;
		int index2 = (index = weaponID);
		index = weaponsAmmo[index];
		list[index2] = index - 1;
		animator.SetBool("Use", value: true);
		audioSource.PlayOneShot(useSound);
		yield return new WaitForSeconds(0.1f);
		FPSController.instance.Health += 100;
		if (FPSController.instance.Health > 100)
		{
			FPSController.instance.Health = 100;
		}
		yield return new WaitForSeconds(0.2f);
		bSetup = false;
		yield return new WaitForSeconds(0.5f);
		if (WeaponManager.instance.weaponsAmmo[weaponID] > 0)
		{
			animator.SetBool("Use", value: false);
			yield break;
		}
		WeaponManager.instance.RemoveWeapon(weaponID);
		UI_System.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}
}
