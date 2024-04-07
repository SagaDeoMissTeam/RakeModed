// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// WeaponManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public List<Transform> playerWeapons = new List<Transform>();

	public List<int> weaponsAmmo = new List<int>();

	public List<Transform> allWeapons = new List<Transform>();

	public int currentWeapon;

	public int currentWeaponID;

	private int maxWeapon;

	public bool bChangeWeapon;

	public Animator anim;

	public Transform weaponSelect;

	public static WeaponManager instance;

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
		if (bChangeWeapon || FPSController.instance.playerState != 0)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (CheckWeapon(1))
			{
				SelectWeaponID(1);
			}
			else
			{
				InformationUI.instance.DrawHint("You do not have rifle");
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (CheckWeapon(6))
			{
				SelectWeaponID(6);
			}
			else
			{
				InformationUI.instance.DrawHint("You do not have this weapon");
			}
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (CheckWeapon(5))
			{
				SelectWeaponID(5);
			}
			else
			{
				InformationUI.instance.DrawHint("You do not have feed");
			}
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (CheckWeapon(4))
			{
				SelectWeaponID(4);
			}
			else
			{
				InformationUI.instance.DrawHint("You do not have trap");
			}
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			if (CheckWeapon(3))
			{
				SelectWeaponID(3);
			}
			else
			{
				InformationUI.instance.DrawHint("You do not have map");
			}
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (CheckWeapon(2))
			{
				SelectWeaponID(2);
			}
			else
			{
				InformationUI.instance.DrawHint("You do not have camera");
			}
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			if (CheckWeapon(7))
			{
				SelectWeaponID(7);
			}
			else
			{
				InformationUI.instance.DrawHint("You do not have binoculars");
			}
		}
	}

	public List<Transform> GetPlayerWeapons()
	{
		return playerWeapons;
	}

	public void SelectWeapon(int id)
	{
		if (playerWeapons.Count > 0)
		{
			if (id > playerWeapons.Count - 1)
			{
				id = playerWeapons.Count - 1;
			}
			StartCoroutine("changeWeapon", id);
		}
	}

	public void SelectWeaponID(int id)
	{
		if (id == currentWeaponID)
		{
			return;
		}
		for (int i = 0; i < playerWeapons.Count; i++)
		{
			if (playerWeapons[i].GetComponent<Weapon>().weaponID == id)
			{
				currentWeaponID = id;
				StartCoroutine("changeWeapon", i);
			}
		}
	}

	public void HideCurrentWeapon()
	{
		HideWeapon(currentWeapon);
	}

	public void HideWeapon(int id)
	{
		StartCoroutine("hideWeapon", id);
	}

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
		currentWeaponID = weaponSelect.GetComponent<Weapon>().weaponID;
		return currentWeaponID;
	}

	public bool CheckWeapon(int id)
	{
		foreach (Transform playerWeapon in playerWeapons)
		{
			if (id == playerWeapon.GetComponent<Weapon>().weaponID)
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
		currentWeapon = i;
		yield return new WaitForSeconds(1f);
		bChangeWeapon = false;
	}
}
