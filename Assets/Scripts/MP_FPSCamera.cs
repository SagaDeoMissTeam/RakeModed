// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_FPSCamera
using UnityEngine;

public class MP_FPSCamera : MonoBehaviour
{
	public Animator anim;

	private void Start()
	{
		if (!GetComponent<NetworkView>().isMine)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 4f))
		{
			Debug.DrawLine(base.transform.position, hitInfo.point);
			if ((bool)hitInfo.collider.GetComponent<MP_Item>())
			{
				MP_Item component = hitInfo.collider.GetComponent<MP_Item>();
				MP_InformationUI.instance.DrawItemInfo("Press E to pickup " + component.name, showType: true);
				if (Input.GetButtonDown("Use"))
				{
					component.PickupItem();
				}
			}
			else
			{
				MP_InformationUI.instance.DrawItemInfo(string.Empty, showType: false);
			}
			if ((bool)hitInfo.collider.GetComponent<MP_DynamicObject>())
			{
				MP_DynamicObject component2 = hitInfo.collider.GetComponent<MP_DynamicObject>();
				MP_InformationUI.instance.DrawItemInfo(component2.name, showType: true);
				if (Input.GetButtonDown("Use"))
				{
					MP_InformationUI.instance.DrawItemInfo(string.Empty, showType: false);
					component2.Action();
				}
			}
		}
		else
		{
			MP_InformationUI.instance.DrawItemInfo(string.Empty, showType: false);
		}
	}
}
