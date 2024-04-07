// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Sniper_item
using UnityEngine;

public class MP_Sniper_item : MP_Item
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
			MP_WeaponManager.instance.GetComponent<NetworkView>().RPC("AddAmmo", RPCMode.All, 4, 2);
			MP_InformationUI.instance.PickupInfo("+ Add ammo of rifle 2");
			Object.Destroy(base.gameObject);
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			MP_WeaponManager.instance.GetComponent<NetworkView>().RPC("AddWeapon", RPCMode.All, 4, 1, true);
			MP_InformationUI.instance.PickupInfo("+ Add rifle 1");
			MP_InformationUI.instance.PickupInfo("+ Add ammo of rifle 2");
			Network.Destroy(base.gameObject);
		}
	}
}
