// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SniperAmmo_item
using UnityEngine;

public class SniperAmmo_item : Item
{
	public override void PickupItem()
	{
		if (WeaponManager.instance.CheckWeapon(6))
		{
			if (WeaponManager.instance.bFullAmmo(6))
			{
				InformationUI.instance.DrawHint("Full ammo in the sniper-rifle");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(6, 6);
			InformationUI.instance.PickupInfo("+ Add ammo of sniper-rifle 6");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(6, 6);
			InformationUI.instance.PickupInfo("+ Add ammo of sniper-rifle 6");
			Object.Destroy(base.gameObject);
		}
	}
}
