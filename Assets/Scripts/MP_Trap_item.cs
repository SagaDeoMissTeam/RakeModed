// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Trap_item
using UnityEngine;

public class MP_Trap_item : MP_Item
{
	public override void PickupItem()
	{
		if (MP_WeaponManager.instance.CheckWeapon(5))
		{
			if (MP_WeaponManager.instance.bFullAmmo(5))
			{
				MP_InformationUI.instance.DrawHint("Full count of traps");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			MP_WeaponManager.instance.AddAmmo(5, 1);
			MP_InformationUI.instance.PickupInfo("+ Add trap 1");
			Network.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			MP_WeaponManager.instance.AddWeapon(5, 1, type: true);
			MP_InformationUI.instance.PickupInfo("+ Add trap 1");
			Network.Destroy(base.gameObject);
		}
	}
}
