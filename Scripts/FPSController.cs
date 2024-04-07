// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// FPSController
using System.Collections;
using UnityEngine;

public class FPSController : MonoBehaviour
{
	public enum Player_state
	{
		Full,
		Freezed,
		Disabled
	}

	public int Health;

	public Animator cameraAnimator;

	public Animator effectAnimator;

	public Animator itemInfo;

	public RectTransform playerHealth;

	public WeaponManager weaponManager;

	public static FPSController instance;

	public FPSCamera fpsCamera;

	public PlayerMovement playerMovement;

	public CameraMouseLook camMouseLook;

	public AudioClip[] painSounds;

	public Camera cam;

	public Camera weapCam;

	public Inventory inventory;

	public bool bUseInventory;

	public AudioListener listener;

	public GameObject playerCanvas;

	public bool bHide;

	public Light flashlight;

	public GameObject loadScreenObj;

	public Player_state playerState;

	public void SetPlayerState(Player_state state)
	{
		playerState = state;
	}

	public void Awake()
	{
	}

	public void Start()
	{
		instance = this;
		Debug.Log("LOADEDLEVEL IS " + Application.loadedLevel);
		if (Application.loadedLevel == 1)
		{
			Debug.Log("Level cannot load");
		}
		else
		{
			StartCoroutine(waitGame());
		}
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		StartCoroutine(loadScreen());
	}

	private IEnumerator waitGame()
	{
		yield return new WaitForSeconds(1f);
		SaveGameSystem.instance.LoadGame();
	}

	private IEnumerator loadScreen()
	{
		yield return new WaitForSeconds(2f);
		effectAnimator.enabled = true;
		listener.enabled = true;
		Object.Destroy(loadScreenObj);
	}

	private void Update()
	{
		playerHealth.sizeDelta = new Vector2((float)Health * 2f, 10f);
		if (Input.GetButtonDown("Inventory"))
		{
			if (!bUseInventory)
			{
				inventory.OpenInventory();
				WeaponManager.instance.HideCurrentWeapon();
				bUseInventory = true;
				camMouseLook.enabled = false;
				playerMovement.enabled = false;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				SetPlayerDisableState("INVENTORY");
			}
			else
			{
				bUseInventory = false;
				inventory.CloseInventory();
				camMouseLook.enabled = true;
				playerMovement.enabled = true;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				SetPlayerDisableState("FULL");
			}
		}
		if (Input.GetButtonDown("Flashlight"))
		{
			SetLightState(!flashlight.enabled);
		}
	}

	public void SetLightState(bool state)
	{
		flashlight.enabled = state;
	}

	public void Event_playerWeaponSelect()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		bUseInventory = false;
		inventory.CloseInventory();
		EnablePlayerMovement();
	}

	public void CameraRander(bool type)
	{
		cam.enabled = type;
		weapCam.enabled = type;
	}

	public void HidePlayerWeapon()
	{
		weaponManager.HideCurrentWeapon();
		if (bUseInventory)
		{
			inventory.CloseInventory();
		}
	}

	public void DisablePlayer()
	{
		HidePlayerWeapon();
		playerCanvas.SetActive(value: false);
		fpsCamera.enabled = false;
		playerMovement.enabled = false;
		camMouseLook.enabled = false;
		itemInfo.SetBool("Show", value: false);
		listener.enabled = false;
		CameraRander(type: false);
		inventory.enabled = false;
		weaponManager.enabled = false;
	}

	public void DisablePlayerMovement()
	{
		camMouseLook.enabled = false;
		playerMovement.enabled = false;
	}

	public void EnablePlayerMovement()
	{
		playerState = Player_state.Full;
		camMouseLook.enabled = true;
		playerMovement.enabled = true;
	}

	public void SetPlayerDisableState(string state)
	{
		switch (state)
		{
		case "INVENTORY":
			playerMovement.enabled = false;
			camMouseLook.enabled = false;
			playerState = Player_state.Disabled;
			break;
		case "CAMERAMODE":
			HidePlayerWeapon();
			playerCanvas.SetActive(value: false);
			fpsCamera.enabled = false;
			playerMovement.enabled = false;
			camMouseLook.enabled = false;
			itemInfo.SetBool("Show", value: false);
			listener.enabled = false;
			CameraRander(type: false);
			inventory.enabled = false;
			weaponManager.enabled = false;
			break;
		case "SCREEMER":
			camMouseLook.bControl = false;
			playerMovement.bCanMove = false;
			inventory.enabled = false;
			playerState = Player_state.Disabled;
			break;
		case "PAUSEMENU":
			HidePlayerWeapon();
			playerState = Player_state.Disabled;
			inventory.enabled = false;
			break;
		case "BEFOREPAUSE":
			playerState = Player_state.Full;
			inventory.enabled = true;
			weaponManager.enabled = true;
			break;
		case "FULL":
			playerState = Player_state.Full;
			inventory.enabled = true;
			playerCanvas.SetActive(value: true);
			listener.enabled = true;
			fpsCamera.enabled = true;
			playerMovement.enabled = true;
			camMouseLook.enabled = true;
			camMouseLook.bControl = true;
			playerMovement.bCanMove = true;
			itemInfo.SetBool("Show", value: true);
			CameraRander(type: true);
			weaponManager.enabled = true;
			break;
		}
	}

	public void EnablePlayer()
	{
		playerState = Player_state.Full;
		inventory.enabled = true;
		playerCanvas.SetActive(value: true);
		listener.enabled = true;
		fpsCamera.enabled = true;
		playerMovement.enabled = true;
		camMouseLook.enabled = true;
		itemInfo.SetBool("Show", value: true);
		CameraRander(type: true);
		weaponManager.enabled = true;
	}

	public void TakeDamage(int amount, string damageType)
	{
		Health -= amount;
		GetComponent<AudioSource>().PlayOneShot(painSounds[Random.Range(0, painSounds.Length)]);
		if (damageType == "TrapDamage")
		{
			StartCoroutine(takeTrapDamage());
		}
		if (damageType == "RakeKick")
		{
			StartCoroutine(TakeDamage());
		}
		if (damageType == "RakeUppercut")
		{
			StartCoroutine(RakeUppercut());
		}
		if (Health < 0)
		{
			KillPlayer();
		}
	}

	public void KillPlayer()
	{
		cameraAnimator.SetFloat("Speed", 0f);
		cameraAnimator.SetBool("Dead", value: true);
		WeaponManager.instance.HideCurrentWeapon();
		playerMovement.bCanMove = false;
		camMouseLook.bControl = false;
		InformationUI.instance.DrawObjective("Game over");
		StartCoroutine(restart());
	}

	private IEnumerator restart()
	{
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(0);
	}

	private IEnumerator takeTrapDamage()
	{
		effectAnimator.SetBool("Blood", value: true);
		cameraAnimator.SetBool("LegHit", value: true);
		yield return new WaitForSeconds(1.5f);
		cameraAnimator.SetBool("LegHit", value: false);
		yield return new WaitForSeconds(3f);
		effectAnimator.SetBool("Blood", value: false);
	}

	private IEnumerator RakeUppercut()
	{
		effectAnimator.SetBool("Blood", value: true);
		cameraAnimator.SetBool("Falldown", value: true);
		yield return new WaitForSeconds(4.5f);
		cameraAnimator.SetBool("Falldown", value: false);
		effectAnimator.SetBool("Blood", value: false);
	}

	private IEnumerator TakeDamage()
	{
		effectAnimator.SetBool("Blood", value: true);
		cameraAnimator.SetBool("Kick", value: true);
		yield return new WaitForSeconds(4.5f);
		cameraAnimator.SetBool("Kick", value: false);
		effectAnimator.SetBool("Blood", value: false);
	}
}
