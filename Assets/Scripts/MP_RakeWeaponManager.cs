// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_RakeWeaponManager
using System.Collections.Generic;
using UnityEngine;

public class MP_RakeWeaponManager : MonoBehaviour
{
	public List<Transform> playerWeapons = new List<Transform>();

	public Transform currentWeapon;

	public List<int> weaponsAmmo = new List<int>();

	public MP_RPSController controller;

	private void Update()
	{
		controller.weaponType = currentWeapon.GetComponent<MP_Weapon>().weaponType;
		if (GetComponent<NetworkView>().isMine)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				GetComponent<NetworkView>().RPC("SelectWeapon", RPCMode.All, 0);
				Debug.Log("Select");
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				GetComponent<NetworkView>().RPC("SelectWeapon", RPCMode.All, 1);
				Debug.Log("Select");
			}
		}
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
}
