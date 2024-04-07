// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_CameraWeapon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MP_CameraWeapon : MP_Weapon
{
	public GameObject FPSWeapon;

	public GameObject TPSWeapon;

	public Animator animator;

	public Animator UI;

	public bool bSetup;

	public Text ammoDisplay;

	private void Start()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			MP_CanvasManager component = GameObject.Find("CanvasManager").GetComponent<MP_CanvasManager>();
			UI_System = component.GetWeaponCanvas(2);
			UI_System.SetActive(value: true);
			UI = UI_System.GetComponent<Animator>();
			ammoDisplay = UI_System.transform.FindChild("Ammo_display").GetComponent<Text>();
		}
	}

	private void OnEnable()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			bSetup = false;
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
		TPSWeapon.SetActive(value: false);
		if ((bool)UI_System)
		{
			UI_System.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (!GetComponent<NetworkView>().isMine)
		{
			return;
		}
		ammo = MP_WeaponManager.instance.weaponsAmmo[weaponID];
		ammoDisplay.text = ammo.ToString();
		Ray ray = new Ray(base.transform.position, base.transform.forward);
        RaycastHit hitInfo;
        if (!Physics.Raycast(ray, out hitInfo, 2.5f) || !hitInfo.collider.GetComponent<MP_RECCamera_obj>())
		{
			return;
		}
		MP_RECCamera_obj component = hitInfo.collider.GetComponent<MP_RECCamera_obj>();
		if (Input.GetButtonDown("Use"))
		{
			MonoBehaviour.print("UsE");
			if (weaponManager.weaponsAmmo[weaponID] > 0 && !bSetup)
			{
				MonoBehaviour.print("UsE");
				StartCoroutine(Setup(component));
			}
		}
	}

	private IEnumerator Setup(MP_RECCamera_obj cam)
	{
		if (cam.CheckSetup())
		{
			bSetup = true;
			List<int> weaponsAmmo;
			List<int> list = (weaponsAmmo = weaponManager.weaponsAmmo);
			int index;
			int index2 = (index = weaponID);
			index = weaponsAmmo[index];
			list[index2] = index - 1;
			animator.SetBool("Setup", value: true);
			yield return new WaitForSeconds(0.1f);
			cam.GetComponent<NetworkView>().RPC("SetupCamera", RPCMode.All);
			yield return new WaitForSeconds(0.2f);
			bSetup = false;
			yield return new WaitForSeconds(0.5f);
			if (weaponManager.weaponsAmmo[weaponID] > 0)
			{
				animator.SetBool("Setup", value: false);
			}
			else
			{
				weaponManager.GetComponent<NetworkView>().RPC("HideCurrentWeapon", RPCMode.All);
			}
		}
	}
}
