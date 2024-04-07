// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// RifleAmmo_item
using UnityEngine;

public class RifleAmmo_item : Item
{
	public override void PickupItem()
	{
		if (WeaponManager.instance.CheckWeapon(1))
		{
			if (WeaponManager.instance.bFullAmmo(1))
			{
				InformationUI.instance.DrawHint("Full ammo in the Rifle");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(1, 6);
			InformationUI.instance.PickupInfo("+ Add ammo of rifle 6");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(1, 6);
			InformationUI.instance.PickupInfo("+ Add ammo of rifle 6");
			Object.Destroy(base.gameObject);
		}
	}
}
