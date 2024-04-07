// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MapCase_weapon
using UnityEngine;

public class MapCase_weapon : Weapon
{
	public Animator animator;

	public GameObject UI_System;

	private void OnEnable()
	{
		UI_System.SetActive(value: true);
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
		if (Input.GetButton("Fire1"))
		{
			animator.SetBool("Zoom", value: true);
		}
		else
		{
			animator.SetBool("Zoom", value: false);
		}
	}
}
