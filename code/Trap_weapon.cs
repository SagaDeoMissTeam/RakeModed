// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Trap_weapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap_weapon : Weapon
{
	public Animator animator;

	public Transform spawnPoint;

	public GameObject projectile;

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
		if (Input.GetButtonDown("Fire1") && !bSetup)
		{
			StartCoroutine(Setup());
		}
	}

	private IEnumerator Setup()
	{
		bSetup = true;
		List<int> weaponsAmmo;
		List<int> list = (weaponsAmmo = WeaponManager.instance.weaponsAmmo);
		int index;
		int index2 = (index = weaponID);
		index = weaponsAmmo[index];
		list[index2] = index - 1;
		animator.SetBool("Setup", value: true);
		yield return new WaitForSeconds(0.7f);
		GameObject obj = (GameObject)Object.Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
		LevelItemsManager.instance.AddTrap(obj);
		yield return new WaitForSeconds(0.2f);
		yield return new WaitForSeconds(0.5f);
		if (WeaponManager.instance.weaponsAmmo[weaponID] > 0)
		{
			animator.SetBool("Setup", value: false);
		}
		else
		{
			WeaponManager.instance.RemoveWeapon(weaponID);
			UI_System.SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
		yield return new WaitForSeconds(1f);
		bSetup = false;
	}
}
