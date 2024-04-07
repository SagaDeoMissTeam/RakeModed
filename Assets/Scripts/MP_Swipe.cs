// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Swipe
using System.Collections;
using UnityEngine;

public class MP_Swipe : MonoBehaviour
{
	public GameObject FPSWeapon;

	public GameObject TPSWeapon;

	public Animator animator;

	public Transform muzzlePointTPS;

	public Animator UI;

	public GameObject UI_System;

	public bool bSight;

	public Transform caster;

	public bool bReload;

	public bool bCanFire = true;

	public AudioSource SFX;

	public AudioClip fireSound;

	[SerializeField]
	public Animation anim;

	public GameObject[] decals;

	public GameObject muzzlePos;

	public Transform firePoint;

	public GameObject decal;

	public GameObject muzzleFire;

	public MP_RPSController controller;

	private void OnEnable()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			FPSWeapon.SetActive(value: true);
			bSight = false;
			bReload = false;
			bCanFire = true;
		}
		else
		{
			FPSWeapon.SetActive(value: false);
		}
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		if (GetComponent<NetworkView>().isMine && Input.GetButtonDown("Fire1") && bCanFire)
		{
			StartCoroutine(Fire());
		}
	}

	private void PlayerFire()
	{
		Debug.Log("FPS FIRE");
		SFX.PlayOneShot(fireSound);
	}

	[RPC]
	private void TPSAttack()
	{
		StartCoroutine(TPSFire());
	}

	private IEnumerator TPSFire()
	{
		controller.weaponType = 1;
		animator.SetInteger("WeaponType", 1);
		yield return new WaitForSeconds(0.7f);
		animator.SetInteger("WeaponType", 0);
		controller.weaponType = 0;
	}

	private IEnumerator Fire()
	{
		bCanFire = false;
		GetComponent<NetworkView>().RPC("TPSAttack", RPCMode.All);
		controller.weaponType = 1;
		animator.SetInteger("WeaponType", 1);
		anim.Play("Take 002");
		yield return new WaitForSeconds(0.2f);
		yield return new WaitForSeconds(0.5f);
		Ray ray = new Ray(caster.position, caster.forward);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 4f))
		{
			if (hit.collider.tag == "Player")
			{
				hit.collider.GetComponent<NetworkView>().RPC("TakeDamage", RPCMode.All, 50);
				Network.Instantiate(decals[1], hit.point + hit.normal * 0.001f, Quaternion.identity, 0);
			}
			else
			{
				Network.Instantiate(decals[0], hit.point + hit.normal * 0.001f, Quaternion.identity, 0);
			}
		}
		controller.weaponType = 0;
		animator.SetInteger("WeaponType", 0);
		anim.Play("Take 001");
		bCanFire = true;
	}
}
