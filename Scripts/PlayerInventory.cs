// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// PlayerInventory
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	public List<int> weaponAmmo = new List<int>();

	public List<int> playerWeapon = new List<int>();

	public PlayerInventory instance;
}
