// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_FPSController
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_FPSController : MP_PlayerController
{
	public enum STATE_TYPE
	{
		Free,
		CameraWatch,
		RakeAttack
	}

	public STATE_TYPE State;

	public Animator animator;

	public Animator cameraAnimator;

	public Animator weaponAnim;

	public float horizontalInput;

	public float verticalInput;

	public float speed;

	public int weaponType;

	public GameObject spawnManager;

	public GameObject playerCanvas;

	public GameObject ThirdPersonObject;

	public List<Behaviour> controlComp = new List<Behaviour>();

	public GameObject[] crossObj;

	public GameObject[] playerSkin;

	public Animator effectAnimator;

	public RectTransform playerHealth_UI;

	public int playerHealth = 100;

	public bool bUse;

	public GameObject ragdollObj;

	public GameObject player;

	public GameObject map;

	public static MP_FPSController instance;

	private void Start()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			instance = this;
			MP_CanvasManager component = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<MP_CanvasManager>();
			effectAnimator = GameObject.FindGameObjectWithTag("Effects_UI").GetComponent<Animator>();
			playerHealth_UI = component.playerHealth;
			base.gameObject.layer = 2;
		}
		else
		{
			base.gameObject.layer = 0;
		}
	}

	public void DisablePlayer()
	{
		for (int i = 0; i < controlComp.Count; i++)
		{
			controlComp[i].enabled = false;
		}
	}

	public void EnablePlayer()
	{
		for (int i = 0; i < controlComp.Count; i++)
		{
			controlComp[i].enabled = true;
		}
	}

	private void Update()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			playerHealth_UI.sizeDelta = new Vector2((float)playerHealth * 2f, 10f);
		}
		cameraAnimator.SetFloat("Speed", speed);
		weaponAnim.SetFloat("Speed", speed);
		if (animator != null)
		{
			animator.SetFloat("HorizontalDirection", horizontalInput);
			animator.SetFloat("VerticalDirection", verticalInput);
			animator.SetFloat("Speed", speed);
			animator.SetInteger("WeaponType", weaponType);
		}
	}

	public void EnableControllScripts()
	{
		base.gameObject.tag = "Player";
		for (int i = 0; i < controlComp.Count; i++)
		{
			controlComp[i].enabled = true;
		}
		ThirdPersonObject.SetActive(value: false);
	}

	public void DisableControllScripts()
	{
		base.gameObject.tag = "Untagged";
		for (int i = 0; i < controlComp.Count; i++)
		{
			controlComp[i].enabled = false;
		}
		ThirdPersonObject.SetActive(value: true);
	}

	[RPC]
	public override void TakeDamage(int amount, string damageType)
	{
		if (damageType == "Kick")
		{
			StartCoroutine(playerKick());
		}
		playerHealth -= amount;
		if (playerHealth <= 0)
		{
			playerHealth = 0;
			if (playerHealth_UI != null)
			{
				playerHealth_UI.sizeDelta = new Vector2((float)playerHealth * 2f, 10f);
			}
			Object.Instantiate(ragdollObj, base.transform.position, base.transform.rotation);
			if (GetComponent<NetworkView>().isMine)
			{
				spawnManager.SetActive(value: true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			Object.Destroy(base.gameObject);
		}
	}

	[RPC]
	public void AddHealth(int amount)
	{
		playerHealth += amount;
		if (playerHealth > 100)
		{
			playerHealth = 100;
		}
	}

	private IEnumerator playerKick()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			effectAnimator.SetBool("Blood", value: true);
			cameraAnimator.SetBool("Kick", value: true);
			yield return new WaitForSeconds(4.5f);
			cameraAnimator.SetBool("Kick", value: false);
			effectAnimator.SetBool("Blood", value: false);
		}
	}

	private void OnDisconnectedFromServer()
	{
		Network.Destroy(base.gameObject);
		Application.LoadLevel(0);
	}
}
