// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SetupCamera
using UnityEngine;
using UnityEngine.UI;

public class SetupCamera : DynamicObject
{
	public Text text;

	private void Update()
	{
		text.text = "Cam state: Rec/Sound: on/Cam ID:" + (CameraManager.instance.currentCameraID + 1);
	}

	public override void Action()
	{
		if (WeaponManager.instance.CheckWeapon(2))
		{
			if (WeaponManager.instance.bFullAmmo(2))
			{
				InformationUI.instance.DrawHint("Full count of cameras");
				return;
			}
			WeaponManager.instance.AddAmmo(2, 1);
			InformationUI.instance.PickupInfo("+ Add camera 1");
			Object.Destroy(base.gameObject);
		}
		else
		{
			WeaponManager.instance.AddWeapon(2, 1, type: true);
			InformationUI.instance.PickupInfo("+ Add camera 1");
			Object.Destroy(base.gameObject);
		}
	}
}
