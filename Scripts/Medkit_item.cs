// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Medkit_item
using UnityEngine;

public class Medkit_item : Item
{
	public override void PickupItem()
	{
		if (WeaponManager.instance.CheckWeapon(8))
		{
			if (WeaponManager.instance.bFullAmmo(8))
			{
				InformationUI.instance.DrawHint("Full count of med kits");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(8, 1);
			InformationUI.instance.PickupInfo("+ Add medkit 1");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddWeapon(8, 1, type: true);
			InformationUI.instance.PickupInfo("+ Add medkit 1");
			Object.Destroy(base.gameObject);
		}
	}
}
