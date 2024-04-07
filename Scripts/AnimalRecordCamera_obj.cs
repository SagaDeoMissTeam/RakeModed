// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AnimalRecordCamera_obj
using UnityEngine;

public class AnimalRecordCamera_obj : RECCameras_obj
{
	public GameObject setupCamera;

	public Transform parent;

	public Transform collar;

	public Transform backlight;

	public override void Action()
	{
		if (WeaponManager.instance.CurrentWeapon() != 2)
		{
			if (WeaponManager.instance.CheckWeapon(2))
			{
				InformationUI.instance.DrawHint("Please select camera");
			}
			else
			{
				InformationUI.instance.DrawHint("You do not have camera");
			}
		}
	}

	private void Update()
	{
		UpdateColor();
	}

	private void UpdateColor()
	{
		Renderer component = backlight.GetComponent<Renderer>();
		Color color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, Mathf.PingPong(Time.time * 0.1f, 0.1f));
		component.material.SetColor("_TintColor", color);
	}

	public override void SetupCamera()
	{
		backlight.gameObject.SetActive(value: false);
		Debug.Log("FF");
		setupCamera.gameObject.SetActive(value: true);
		collar.gameObject.SetActive(value: true);
		CameraManager.instance.AddCamera(setupCamera.transform);
		Object.Destroy(base.gameObject);
	}
}
