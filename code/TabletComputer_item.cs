// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// TabletComputer_item
using UnityEngine;

public class TabletComputer_item : Item
{
	public override void PickupItem()
	{
		if (WeaponManager.instance.CheckWeapon(3))
		{
			if (WeaponManager.instance.bFullAmmo(3))
			{
				InformationUI.instance.DrawHint("You have this");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(3, 1);
			InformationUI.instance.PickupInfo("+ Add tablet-computer");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddWeapon(3, 1, type: false);
			InformationUI.instance.PickupInfo("+ Add tablet-computer 1");
			Object.Destroy(base.gameObject);
		}
	}
}
