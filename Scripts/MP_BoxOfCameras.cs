// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_BoxOfCameras
using UnityEngine;

public class MP_BoxOfCameras : MP_Item
{
	public override void PickupItem()
	{
		if (MP_WeaponManager.instance.CheckWeapon(2))
		{
			if (MP_WeaponManager.instance.bFullAmmo(2))
			{
				MP_InformationUI.instance.DrawHint("Full count of cameras");
				return;
			}
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			MP_WeaponManager.instance.GetComponent<NetworkView>().RPC("AddAmmo", RPCMode.All, 2, 1);
			MP_InformationUI.instance.PickupInfo("+ Add camera 1");
		}
		else
		{
			AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
			MP_WeaponManager.instance.GetComponent<NetworkView>().RPC("AddWeapon", RPCMode.All, 2, 1, true);
			MP_InformationUI.instance.PickupInfo("+ Add camera 1");
		}
	}
}
