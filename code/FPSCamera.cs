// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// FPSCamera
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
	public Animator anim;

	private void Update()
	{
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		if (Physics.Raycast(ray, out var hitInfo, 4f))
		{
			Debug.DrawLine(base.transform.position, hitInfo.point);
			if ((bool)hitInfo.collider.GetComponent<Item>())
			{
				Item component = hitInfo.collider.GetComponent<Item>();
				InformationUI.instance.DrawItemInfo("Press E to pickup " + component.name, showType: true);
				if (Input.GetButtonDown("Use"))
				{
					component.PickupItem();
				}
			}
			else
			{
				InformationUI.instance.DrawItemInfo(string.Empty, showType: false);
			}
			if ((bool)hitInfo.collider.GetComponent<DynamicObject>())
			{
				DynamicObject component2 = hitInfo.collider.GetComponent<DynamicObject>();
				InformationUI.instance.DrawItemInfo(component2.name, showType: true);
				if (Input.GetButtonDown("Use"))
				{
					component2.Action();
				}
			}
		}
		else
		{
			InformationUI.instance.DrawItemInfo(string.Empty, showType: false);
		}
	}
}
