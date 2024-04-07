// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Medkit_item
using UnityEngine;

public class MP_Medkit_item : MP_Item
{
	public override void PickupItem()
	{
		MP_WeaponManager.instance.gameObject.GetComponent<NetworkView>().RPC("AddHealth", RPCMode.All, 100);
		MP_InformationUI.instance.DrawHint("Health has been restore");
		AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
		Network.Destroy(base.gameObject);
	}
}
