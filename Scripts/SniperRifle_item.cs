// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SniperRifle_item
using UnityEngine;

public class SniperRifle_item : Item
{
	public override void PickupItem()
	{
		if (WeaponManager.instance.CheckWeapon(6))
		{
			if (WeaponManager.instance.bFullAmmo(6))
			{
				InformationUI.instance.DrawHint("Full ammo in the Rifle");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(6, 2);
			InformationUI.instance.PickupInfo("+ Add ammo of rifle 2");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddWeapon(6, 2, type: true);
			InformationUI.instance.PickupInfo("+ Add rifle 1");
			InformationUI.instance.PickupInfo("+ Add ammo of rifle 2");
			Object.Destroy(base.gameObject);
		}
	}
}
