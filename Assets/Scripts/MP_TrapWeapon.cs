// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_TrapWeapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MP_TrapWeapon : MP_Weapon
{
	public Animator animator;

	public GameObject FPSWeapon;

	public GameObject TPSWeapon;

	public Transform spawnPoint;

	public GameObject projectile;

	public Animator UI;

	public bool bSetup;

	public Text ammoDisplay;

	public GameObject text;

	private void Start()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			MP_CanvasManager component = GameObject.Find("CanvasManager").GetComponent<MP_CanvasManager>();
			UI_System = component.GetWeaponCanvas(5);
			UI_System.SetActive(value: true);
			UI = UI_System.GetComponent<Animator>();
			text = UI_System.transform.FindChild("Ammo_display").gameObject;
			ammoDisplay = text.GetComponent<Text>();
			animator.SetBool("Sight", value: true);
		}
	}

	private void OnEnable()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			if (UI_System != null)
			{
				UI_System.SetActive(value: true);
			}
			FPSWeapon.SetActive(value: true);
			TPSWeapon.SetActive(value: false);
		}
		else
		{
			FPSWeapon.SetActive(value: false);
			TPSWeapon.SetActive(value: true);
		}
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
		if (GetComponent<NetworkView>().isMine)
		{
			ammo = weaponManager.weaponsAmmo[weaponID];
			ammoDisplay.GetComponent<Text>().text = ammo.ToString();
			if (Input.GetButtonDown("Fire1") && !bSetup)
			{
				StartCoroutine(Setup());
			}
		}
	}

	private IEnumerator Setup()
	{
		bSetup = true;
		List<int> weaponsAmmo;
		List<int> list = (weaponsAmmo = weaponManager.weaponsAmmo);
		int index;
		int index2 = (index = weaponID);
		index = weaponsAmmo[index];
		list[index2] = index - 1;
		animator.SetBool("Setup", value: true);
		yield return new WaitForSeconds(0.7f);
		GameObject obj = (GameObject)Network.Instantiate(projectile, spawnPoint.position, spawnPoint.rotation, 0);
		yield return new WaitForSeconds(0.2f);
		yield return new WaitForSeconds(0.5f);
		if (weaponManager.weaponsAmmo[weaponID] > 0)
		{
			animator.SetBool("Setup", value: false);
		}
		else
		{
			weaponManager.RemoveWeapon(weaponID);
			UI_System.SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
		yield return new WaitForSeconds(1f);
		bSetup = false;
	}
}
