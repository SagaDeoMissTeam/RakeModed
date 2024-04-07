// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Trap_item
using UnityEngine;

public class Trap_item : Item
{
	public override void PickupItem()
	{
		if (WeaponManager.instance.CheckWeapon(4))
		{
			if (WeaponManager.instance.bFullAmmo(4))
			{
				InformationUI.instance.DrawHint("Full count of traps");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(4, 1);
			InformationUI.instance.PickupInfo("+ Add trap 1");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddWeapon(4, 1, type: true);
			InformationUI.instance.PickupInfo("+ Add trap 1");
			Object.Destroy(base.gameObject);
		}
	}
}
