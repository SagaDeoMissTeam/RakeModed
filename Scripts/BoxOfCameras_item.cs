// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// BoxOfCameras_item
using UnityEngine;

public class BoxOfCameras_item : Item
{
	public Objective objective;

	public GameObject obj;

	public override void PickupItem()
	{
		if (count > 0)
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
				count--;
				if (count <= 0)
				{
					obj.SetActive(value: false);
				}
				if (objective != null)
				{
					objective.ObjectiveAction();
				}
			}
			else
			{
				AudioSource.PlayClipAtPoint(pickupSound, base.transform.position);
				WeaponManager.instance.AddWeapon(2, 1, type: true);
				InformationUI.instance.PickupInfo("+ Add camera 1");
				if (objective != null)
				{
					objective.ObjectiveAction();
				}
				count--;
				if (count <= 0)
				{
					obj.SetActive(value: false);
				}
			}
		}
		else
		{
			InformationUI.instance.DrawHint("Empty");
			obj.SetActive(value: false);
		}
	}
}
