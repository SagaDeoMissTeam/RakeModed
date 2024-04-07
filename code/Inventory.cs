// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Inventory
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public List<GameObject> items = new List<GameObject>();

	public List<Transform> playerItems = new List<Transform>();

	public RectTransform pos;

	public GameObject drawer;

	public Text text;

	public bool inventoryState;

	public GameObject inventoryObj;

	public AudioClip openSound;

	public AudioClip closeSound;

	public AudioSource audioSource;

	private void RefreshItems()
	{
		playerItems = WeaponManager.instance.GetPlayerWeapons();
		foreach (GameObject item in items)
		{
			item.gameObject.SetActive(value: false);
		}
		for (int i = 0; i < playerItems.Count; i++)
		{
			int weaponID = playerItems[i].GetComponent<Weapon>().weaponID;
			items[weaponID].SetActive(value: true);
			items[weaponID].GetComponent<Inventory_item>().count = playerItems[i].GetComponent<Weapon>().ammo;
		}
	}

	public void OpenInventory()
	{
		audioSource.PlayOneShot(openSound);
		drawer.SetActive(value: true);
		inventoryObj.SetActive(value: true);
		inventoryState = true;
		RefreshItems();
	}

	public void CloseInventory()
	{
		audioSource.PlayOneShot(closeSound);
		inventoryObj.SetActive(value: false);
		drawer.SetActive(value: false);
		inventoryState = false;
	}

	private void Update()
	{
		if (!inventoryState)
		{
			return;
		}
		pos.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y - pos.sizeDelta.y);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out var hitInfo, 100f))
		{
			if ((bool)hitInfo.collider.GetComponent<Inventory_item>())
			{
				string text = hitInfo.collider.GetComponent<Inventory_item>().itemType.ToString();
				drawer.SetActive(value: true);
				this.text.text = "Name: " + hitInfo.collider.GetComponent<Inventory_item>().name + "\r\n" + text + " " + hitInfo.collider.GetComponent<Inventory_item>().count;
				if (Input.GetButtonDown("Fire1"))
				{
					WeaponManager.instance.SelectWeaponID(hitInfo.collider.GetComponent<Inventory_item>().ID);
					FPSController.instance.Event_playerWeaponSelect();
				}
			}
			else
			{
				drawer.SetActive(value: false);
			}
		}
		else
		{
			drawer.SetActive(value: false);
		}
	}
}
