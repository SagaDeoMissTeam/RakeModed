// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Rifle
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MP_Rifle : MP_Weapon
{
	public GameObject FPSWeapon;

	public GameObject TPSWeapon;

	public Animator animator;

	public Transform muzzlePointTPS;

	public Animator UI;

	public bool bSight;

	public GameObject text;

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

	public Transform firePoint;

	public GameObject decal;

	public GameObject muzzleFire;

	private void Start()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			MP_CanvasManager component = GameObject.Find("CanvasManager").GetComponent<MP_CanvasManager>();
			UI_System = component.GetWeaponCanvas(1);
			UI_System.SetActive(value: true);
			UI = UI_System.GetComponent<Animator>();
			text = UI_System.transform.FindChild("Ammo_display").gameObject;
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
				UI.SetInteger("Bullets", currentAmmo);
			}
			FPSWeapon.SetActive(value: true);
			TPSWeapon.SetActive(value: false);
			bSight = false;
			bReload = false;
			bCanFire = true;
			firePoint = muzzlePos.transform;
		}
		else
		{
			FPSWeapon.SetActive(value: false);
			TPSWeapon.SetActive(value: true);
			firePoint = muzzlePointTPS;
		}
	}

	private void OnDisable()
	{
		TPSWeapon.SetActive(value: false);
		if ((bool)UI_System)
		{
			UI_System.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (!GetComponent<NetworkView>().isMine)
		{
			return;
		}
		ammo = weaponManager.weaponsAmmo[weaponID];
		text.GetComponent<Text>().text = ammo.ToString();
		animator.SetBool("Sight", value: true);
		if (Input.GetButtonDown("Fire1") && bCanFire && currentAmmo > 0)
		{
			PlayerFire();
			GetComponent<NetworkView>().RPC("TPSFire", RPCMode.Others);
			StartCoroutine(Fire());
			Ray ray = new Ray(caster.position, caster.forward);
			RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1000f))
			{
				if (hitInfo.collider.tag == "Player")
				{
					hitInfo.collider.GetComponent<NetworkView>().RPC("TakeDamage", RPCMode.All, 20, "Fire");
					Network.Instantiate(decals[1], hitInfo.point + hitInfo.normal * 0.001f, Quaternion.identity, 0);
				}
				else
				{
					Network.Instantiate(decals[0], hitInfo.point + hitInfo.normal * 0.001f, Quaternion.identity, 0);
				}
				if (hitInfo.collider.tag == "Rake")
				{
					hitInfo.collider.GetComponent<NetworkView>().RPC("TakeDamage", RPCMode.All, 100);
					Network.Instantiate(decals[1], hitInfo.point + hitInfo.normal * 0.001f, Quaternion.identity, 0);
				}
				if ((bool)hitInfo.collider.GetComponent<MP_Animal>())
				{
					hitInfo.collider.GetComponent<NetworkView>().RPC("TakeDamage", RPCMode.All, 100);
					Network.Instantiate(decals[1], hitInfo.point + hitInfo.normal * 0.001f, Quaternion.identity, 0);
				}
			}
		}
		if (Input.GetButtonDown("Reload") && currentAmmo < 2 && weaponManager.weaponsAmmo[weaponID] > 0 && !bReload)
		{
			Debug.Log("reload");
			StartCoroutine(Reload());
		}
	}

	private void PlayerFire()
	{
		Debug.Log("FPS FIRE");
		Object.Instantiate(muzzleFire, muzzlePos.transform.position, muzzlePos.transform.rotation);
		SFX.PlayOneShot(fireSound);
	}

	[RPC]
	private void TPSFire()
	{
		Object.Instantiate(muzzleFire, muzzlePointTPS.transform.position, muzzlePointTPS.transform.rotation);
		SFX.PlayOneShot(fireSound);
	}

	private IEnumerator Reload()
	{
		bCanFire = false;
		animator.SetBool("Reload", value: true);
		bReload = true;
		yield return new WaitForSeconds(0.5f);
		SFX.PlayOneShot(reloadSound);
		yield return new WaitForSeconds(1.5f);
		int addBullet = ((weaponManager.weaponsAmmo[weaponID] - (clipSize - currentAmmo) < 0) ? 1 : (clipSize - currentAmmo));
		List<int> weaponsAmmo;
		List<int> list = (weaponsAmmo = weaponManager.weaponsAmmo);
		int index;
		int index2 = (index = weaponID);
		index = weaponsAmmo[index];
		list[index2] = index - (clipSize - currentAmmo);
		if (weaponManager.weaponsAmmo[weaponID] < 0)
		{
			weaponManager.weaponsAmmo[weaponID] = 0;
		}
		currentAmmo += addBullet;
		UI.SetInteger("Bullets", currentAmmo);
		bCanFire = true;
		bReload = false;
		animator.SetBool("Reload", value: false);
	}

	private IEnumerator Fire()
	{
		bCanFire = false;
		animator.SetBool("Fire", value: true);
		currentAmmo--;
		UI.SetInteger("Bullets", currentAmmo);
		yield return new WaitForSeconds(0.2f);
		animator.SetBool("Fire", value: false);
		yield return new WaitForSeconds(1f);
		bCanFire = true;
	}
}
