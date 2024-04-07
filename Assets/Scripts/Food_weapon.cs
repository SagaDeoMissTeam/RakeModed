// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Food_weapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food_weapon : Weapon
{
	public Animator animator;

	public AudioSource audioSource;

	public AudioClip use;

	public Transform spawnPoint;

	public GameObject projectile;

	public Animator UI;

	public GameObject UI_System;

	public Transform sight;

	public bool bSetup;

	public Text ammoDisplay;

	private bool bCanSetup;

	public Transform camera;

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
		if ((bool)sight)
		{
			sight.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		ammo = WeaponManager.instance.weaponsAmmo[weaponID];
		ammoDisplay.text = ammo.ToString();
		Ray ray = new Ray(camera.position, camera.forward);
		RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 3f))
		{
			if ((bool)hitInfo.collider.GetComponent<TerrainCollider>() && hitInfo.collider.name == Terrain.activeTerrain.name)
			{
				float num = Terrain.activeTerrain.SampleHeight(hitInfo.point);
				if (hitInfo.point.y <= num + 0.01f)
				{
					bCanSetup = true;
					sight.gameObject.SetActive(value: true);
					sight.position = hitInfo.point;
				}
				else
				{
					bCanSetup = false;
				}
			}
			else
			{
				bCanSetup = false;
				sight.gameObject.SetActive(value: false);
			}
		}
		else
		{
			bCanSetup = false;
			sight.gameObject.SetActive(value: false);
		}
		if (Input.GetButtonDown("Fire1") && !bSetup && bCanSetup)
		{
			StartCoroutine(Setup());
		}
	}

	private IEnumerator Setup()
	{
		Ray ray = new Ray(camera.position, camera.forward);
		RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f) && (bool)hit.collider.GetComponent<TerrainCollider>())
		{
			bSetup = true;
			List<int> weaponsAmmo;
			List<int> list = (weaponsAmmo = WeaponManager.instance.weaponsAmmo);
			int index;
			int index2 = (index = weaponID);
			index = weaponsAmmo[index];
			list[index2] = index - 1;
			animator.SetBool("Setup", value: true);
			audioSource.PlayOneShot(use);
			yield return new WaitForSeconds(0.7f);
			GameObject point = (GameObject)Object.Instantiate(position: hit.point, original: projectile, rotation: Quaternion.identity);
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
}
