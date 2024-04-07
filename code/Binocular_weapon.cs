// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Binocular_weapon
using UnityEngine;

public class Binocular_weapon : Weapon
{
	public Animator animator;

	public GameObject UI_System;

	public Camera camera;

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
