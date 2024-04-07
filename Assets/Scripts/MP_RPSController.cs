// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_RPSController
using System.Collections.Generic;
using UnityEngine;

public class MP_RPSController : MP_PlayerController
{
	public Animator animator;

	public Animator cameraAnimator;

	public Animator weaponAnim;

	public float horizontalInput;

	public float verticalInput;

	public float speed;

	public bool grounded;

	public int weaponType;

	public GameObject spawnManager;

	public GameObject playerCanvas;

	public GameObject ThirdPersonObject;

	public List<Behaviour> controlComp = new List<Behaviour>();

	public GameObject[] crossObj;

	public GameObject[] playerSkin;

	public RectTransform playerHealth_UI;

	public int playerHealth = 100;

	public bool bUse;

	public GameObject ragdollObj;

	public GameObject player;

	public GameObject map;

	private void Start()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			MP_CanvasManager component = GameObject.Find("CanvasManager").GetComponent<MP_CanvasManager>();
			playerHealth_UI = component.playerHealth;
			base.gameObject.layer = 2;
		}
		else
		{
			base.gameObject.layer = 0;
		}
	}

	private void Update()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			playerHealth_UI.sizeDelta = new Vector2((float)playerHealth * 2f, 10f);
		}
		if (animator != null)
		{
			animator.SetFloat("HorizontalDirection", horizontalInput);
			animator.SetFloat("VerticalDirection", verticalInput);
			animator.SetBool("Grounded", grounded);
			animator.SetFloat("Speed", speed);
			animator.SetInteger("WeaponType", weaponType);
		}
		if (GetComponent<NetworkView>().isMine)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				bUse = true;
			}
			else
			{
				bUse = false;
			}
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
		base.gameObject.tag = "Player";
		for (int i = 0; i < controlComp.Count; i++)
		{
			controlComp[i].enabled = false;
		}
		ThirdPersonObject.SetActive(value: true);
	}

	[RPC]
	public void TakeDamage(int damageTaken)
	{
		MonoBehaviour.print("Damage");
		playerHealth -= damageTaken;
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
			}
			Object.Destroy(base.gameObject);
		}
	}

	private void OnPlayerDisconnected()
	{
		Object.Destroy(base.gameObject);
	}

	private void OnDisconnectedFromServer()
	{
		Object.Destroy(base.gameObject);
	}
}
