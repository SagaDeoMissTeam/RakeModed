// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Camera_item
using UnityEngine;

public class Camera_item : Item
{
	public override void PickupItem()
	{
		if (WeaponManager.instance.CheckWeapon(2))
		{
			if (WeaponManager.instance.bFullAmmo(2))
			{
				InformationUI.instance.DrawHint("Full count of cameras");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddAmmo(2, 1);
			InformationUI.instance.PickupInfo("+ Add camera 1");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			WeaponManager.instance.AddWeapon(2, 1, type: true);
			InformationUI.instance.PickupInfo("+ Add camera 1");
			Object.Destroy(base.gameObject);
		}
	}
}
