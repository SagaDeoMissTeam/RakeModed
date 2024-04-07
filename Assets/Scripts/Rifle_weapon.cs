// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Rifle_weapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rifle_weapon : Weapon
{
	public Animator animator;

	public Animator UI;

	public GameObject UI_System;

	public bool bSight;

	private GameObject text;

	public Transform caster;

	public bool bReload;

	public bool bCanFire = true;

	public AudioSource SFX;

	public AudioClip fireSound;

	public AudioClip reloadSound;

	public int currentAmmo = 2;

	public int clipSize = 2;

	public GameObject[] decals;

	public GameObject muzzlePos;

	public GameObject decal;

	public GameObject muzzleFire;

	private void OnEnable()
	{
		UI_System.SetActive(value: true);
		bSight = false;
		bReload = false;
		bCanFire = true;
		UI.SetInteger("Bullets", currentAmmo);
	}

	private void OnDisable()
	{
		if ((bool)UI_System)
		{
			UI_System.SetActive(value: false);
		}
	}

	private void Start()
	{
		text = UI_System.transform.FindChild("Ammo_display").gameObject;
	}

	private void Update()
	{
		ammo = WeaponManager.instance.weaponsAmmo[weaponID];
		text.GetComponent<Text>().text = ammo.ToString();
		if (Input.GetButton("Fire2"))
		{
			bSight = true;
		}
		else
		{
			bSight = false;
		}
		if (Input.GetButtonDown("Fire1") && bSight && bCanFire && currentAmmo > 0)
		{
			StartCoroutine(Fire());
			Ray ray = new Ray(caster.position, caster.forward);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 1000f))
			{
				NotificationsManager.Instance.PostNotification(this, "Event_PlayerFire");
				if ((bool)hitInfo.collider.GetComponent<Unit_obj>())
				{
					hitInfo.collider.GetComponent<Unit_obj>().TakeDamage(100, "SniperShot");
					Object.Instantiate(decals[1], hitInfo.point + hitInfo.normal * 0.001f, Quaternion.identity);
				}
				if ((bool)hitInfo.collider.GetComponent<Window_obj>())
				{
					hitInfo.collider.GetComponent<Window_obj>().TakeDamage(100, "SniperShot");
				}
				Object.Instantiate(decals[0], hitInfo.point + hitInfo.normal * 0.001f, Quaternion.identity);
			}
		}
		if (Input.GetButtonDown("Reload") && currentAmmo < 2 && WeaponManager.instance.weaponsAmmo[weaponID] > 0 && !bReload)
		{
			Debug.Log("reload");
			StartCoroutine(Reload());
		}
		if (bSight)
		{
			animator.SetBool("Sight", value: true);
		}
		else
		{
			animator.SetBool("Sight", value: false);
		}
	}

	private IEnumerator Fire()
	{
		bCanFire = false;
		Object.Instantiate(muzzleFire, muzzlePos.transform.position, muzzlePos.transform.rotation);
		animator.SetBool("Fire", value: true);
		currentAmmo--;
		SFX.PlayOneShot(fireSound);
		UI.SetInteger("Bullets", currentAmmo);
		yield return new WaitForSeconds(0.2f);
		animator.SetBool("Fire", value: false);
		yield return new WaitForSeconds(1f);
		bCanFire = true;
	}

	private IEnumerator Reload()
	{
		bCanFire = false;
		animator.SetBool("Reload", value: true);
		bReload = true;
		yield return new WaitForSeconds(0.5f);
		SFX.PlayOneShot(reloadSound);
		yield return new WaitForSeconds(1.5f);
		int addBullet = ((WeaponManager.instance.weaponsAmmo[weaponID] - (clipSize - currentAmmo) < 0) ? 1 : (clipSize - currentAmmo));
		List<int> weaponsAmmo;
		List<int> list = (weaponsAmmo = WeaponManager.instance.weaponsAmmo);
		int index;
		int index2 = (index = weaponID);
		index = weaponsAmmo[index];
		list[index2] = index - (clipSize - currentAmmo);
		if (WeaponManager.instance.weaponsAmmo[weaponID] < 0)
		{
			WeaponManager.instance.weaponsAmmo[weaponID] = 0;
		}
		currentAmmo += addBullet;
		UI.SetInteger("Bullets", currentAmmo);
		bCanFire = true;
		bReload = false;
		animator.SetBool("Reload", value: false);
	}
}
