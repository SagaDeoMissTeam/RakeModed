// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_SniperAmmo_item
using UnityEngine;

public class MP_SniperAmmo_item : MP_Item
{
	public override void PickupItem()
	{
		if (MP_WeaponManager.instance.CheckWeapon(4))
		{
			if (MP_WeaponManager.instance.bFullAmmo(4))
			{
				MP_InformationUI.instance.DrawHint("Full ammo in the Rifle");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			MP_WeaponManager.instance.AddAmmo(4, 6);
			MP_InformationUI.instance.PickupInfo("+ Add ammo of rifle 6");
			Network.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			MP_WeaponManager.instance.AddAmmo(4, 6);
			MP_InformationUI.instance.PickupInfo("+ Add ammo of rifle 6");
			Network.Destroy(base.gameObject);
		}
	}
}
