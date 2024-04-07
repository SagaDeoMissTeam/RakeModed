// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_SetupCamera
using UnityEngine;
using UnityEngine.UI;

public class MP_SetupCamera : MP_DynamicObject
{
	public Text text;

	private void Update()
	{
		text.text = "Cam state: Rec/Sound: on/Cam ID:" + (MP_CameraManager.instance.currentCameraID + 1);
	}

	public override void Action()
	{
		if (MP_WeaponManager.instance.CheckWeapon(2))
		{
			if (!MP_WeaponManager.instance.bFullAmmo(2))
			{
				MP_WeaponManager.instance.AddAmmo(2, 1);
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			MP_WeaponManager.instance.AddWeapon(2, 1, type: true);
			Object.Destroy(base.gameObject);
		}
	}
}
