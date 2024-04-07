// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Inventory_item
using UnityEngine;

public class Inventory_item : MonoBehaviour
{
	public enum ItemType
	{
		Count,
		Ammo
	}

	public new string name;

	public int ID;

	public int count;

	public ItemType itemType;
}
