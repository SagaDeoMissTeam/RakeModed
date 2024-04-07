// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Binocular_weapon
using UnityEngine;

public class MP_Binocular_weapon : MP_Weapon
{
	public Animator animator;

	public Camera camera;

	public GameObject FPSWeapon;

	public GameObject TPSWeapon;

	private void Start()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			MP_CanvasManager component = GameObject.Find("CanvasManager").GetComponent<MP_CanvasManager>();
			UI_System = component.GetWeaponCanvas(2);
			UI_System.SetActive(value: true);
		}
	}

	private void OnEnable()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			FPSWeapon.SetActive(value: true);
			TPSWeapon.SetActive(value: false);
			if (UI_System != null)
			{
				UI_System.SetActive(value: true);
			}
		}
		else
		{
			TPSWeapon.SetActive(value: true);
			FPSWeapon.SetActive(value: false);
			if ((bool)UI_System)
			{
				UI_System.SetActive(value: false);
			}
		}
	}

	private void OnDisable()
	{
		if ((bool)UI_System)
		{
			UI_System.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			if (Input.GetMouseButton(0))
			{
				animator.SetBool("Sight", value: true);
				camera.enabled = true;
			}
			else
			{
				animator.SetBool("Sight", value: false);
				camera.enabled = false;
			}
		}
	}
}
