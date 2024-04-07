// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_WeaponManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_WeaponManager : MonoBehaviour
{
	public List<Transform> playerWeapons = new List<Transform>();

	public Transform currentWeapon;

	public List<int> weaponsAmmo = new List<int>();

	public MP_FPSController controller;

	public List<Transform> canvas_UI = new List<Transform>();

	public static MP_WeaponManager instance;

	public int currentWeaponID;

	public List<Transform> allWeapons = new List<Transform>();

	private int maxWeapon;

	public bool bChangeWeapon;

	public Animator anim;

	public Transform weaponSelect;

	private void Start()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			instance = this;
		}
	}

	private void Update()
	{
		controller.weaponType = currentWeapon.GetComponent<MP_Weapon>().weaponType;
		if (GetComponent<NetworkView>().isMine)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SelectWeaponID(1);
				Debug.Log("Select");
			}
			if (Input.GetKeyDown(KeyCode.Alpha2) && CheckWeapon(4))
			{
				SelectWeaponID(4);
				Debug.Log("Select");
			}
			if (Input.GetKeyDown(KeyCode.Alpha3) && weaponsAmmo[2] > 0)
			{
				SelectWeaponID(2);
				Debug.Log("Select");
			}
			if (Input.GetKeyDown(KeyCode.Alpha4) && weaponsAmmo[5] > 0)
			{
				SelectWeaponID(5);
				Debug.Log("Select");
			}
			if (Input.GetButtonDown("Binoculars"))
			{
				SelectWeaponID(3);
			}
		}
	}

	public List<Transform> GetPlayerWeapons()
	{
		return playerWeapons;
	}

	public void SelectWeaponID(int id)
	{
		for (int i = 0; i < playerWeapons.Count; i++)
		{
			if (playerWeapons[i].GetComponent<MP_Weapon>().weaponID == id)
			{
				GetComponent<NetworkView>().RPC("SelectWeapon", RPCMode.All, i);
			}
		}
	}

	[RPC]
	public void AddWeapon(int id, int count, bool type)
	{
		if (CheckWeapon(id))
		{
			AddAmmo(id, count);
			return;
		}
		playerWeapons.Add(allWeapons[id]);
		AddAmmo(id, count);
	}

	public void RemoveWeapon(int id)
	{
		for (int i = 0; i < playerWeapons.Count; i++)
		{
			if (id == playerWeapons[i].GetComponent<Weapon>().weaponID)
			{
				if (id == currentWeaponID)
				{
					currentWeaponID = 0;
					weaponSelect = playerWeapons[0];
				}
				playerWeapons.Remove(playerWeapons[i]);
			}
		}
	}

	public int CurrentWeapon()
	{
		currentWeaponID = currentWeapon.GetComponent<MP_Weapon>().weaponID;
		return currentWeaponID;
	}

	public bool CheckWeapon(int id)
	{
		foreach (Transform playerWeapon in playerWeapons)
		{
			if (id == playerWeapon.GetComponent<MP_Weapon>().weaponID)
			{
				return true;
			}
		}
		return false;
	}

	public bool bFullAmmo(int id)
	{
		if (weaponsAmmo[id] < 1000)
		{
			return false;
		}
		return true;
	}

	[RPC]
	public void AddAmmo(int id, int count)
	{
		List<int> list;
		List<int> list2 = (list = weaponsAmmo);
		int index;
		int index2 = (index = id);
		index = list[index];
		list2[index2] = index + count;
	}

	private IEnumerator hideWeapon(int i)
	{
		anim.SetBool("ChangeWeapon", value: true);
		bChangeWeapon = true;
		yield return new WaitForSeconds(0.5f);
		anim.SetBool("ChangeWeapon", value: false);
		bChangeWeapon = false;
		playerWeapons[i].gameObject.SetActive(value: false);
		currentWeaponID = 0;
		SelectWeapon(0);
	}

	private IEnumerator changeWeapon(int i)
	{
		anim.SetBool("ChangeWeapon", value: true);
		bChangeWeapon = true;
		yield return new WaitForSeconds(0.5f);
		weaponSelect.gameObject.SetActive(value: false);
		anim.SetBool("ChangeWeapon", value: false);
		playerWeapons[i].gameObject.SetActive(value: true);
		weaponSelect = playerWeapons[i];
		yield return new WaitForSeconds(1f);
		bChangeWeapon = false;
	}

	[RPC]
	public void SelectWeapon(int id)
	{
		for (int i = 0; i < playerWeapons.Count; i++)
		{
			if (i == id)
			{
				currentWeapon.gameObject.SetActive(value: false);
				playerWeapons[i].gameObject.SetActive(value: true);
				currentWeapon = playerWeapons[i];
			}
		}
	}

	[RPC]
	public void HideCurrentWeapon()
	{
		GetComponent<NetworkView>().RPC("SelectWeapon", RPCMode.All, 0);
	}
}
