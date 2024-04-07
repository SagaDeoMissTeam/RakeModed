// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Food_item
using UnityEngine;

public class Food_item : Item
{
	public override void PickupItem()
	{
		if (WeaponManager.instance.CheckWeapon(5))
		{
			if (WeaponManager.instance.bFullAmmo(5))
			{
				InformationUI.instance.DrawHint("Full count of feed");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(5, 1);
			InformationUI.instance.PickupInfo("+ Add packet of feed 1");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddWeapon(5, 1, type: true);
			InformationUI.instance.PickupInfo("+ Add packet of feed 1");
			Object.Destroy(base.gameObject);
		}
	}
}
