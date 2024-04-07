// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SaveGameSystem
using UnityEngine;

public class SaveGameSystem : MonoBehaviour
{
	public static SaveGameSystem instance;

	public GameObject trapPrefab;

	private void Start()
	{
		instance = this;
		SaveCurrentLevel();
	}

	private void Update()
	{
	}

	public void SaveGame()
	{
		SaveWindows();
		SavePlayerWeapon();
		SavePlayerAmmo();
		SaveGameCameras();
		SaveTrapPosition();
		SaveItems();
		SaveScreemers();
		SaveSoundTriggers();
		SaveRakeHealth();
		SavePlayerHealth();
		SaveCurrentLevel();
		Debug.Log("Game saved");
	}

	private void SaveWindows()
	{
		int count = LevelItemsManager.instance.windowsObj.Count;
		for (int i = 0; i < count; i++)
		{
			PlayerPrefs.SetInt("Window:" + i, LevelItemsManager.instance.windowsObj[i].GetComponent<Window_obj>().bDestroy);
		}
		Debug.Log("Saved glass");
	}

	private void SaveCurrentLevel()
	{
		PlayerPrefs.SetInt("Level:", Application.loadedLevel);
	}

	private void SaveRakeHealth()
	{
		Enemy_AI component = GameObject.FindGameObjectWithTag("Rake").GetComponent<Enemy_AI>();
		PlayerPrefs.SetFloat("RakeHealth:", component.Health);
	}

	private void SavePlayerHealth()
	{
		FPSController component = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
		PlayerPrefs.SetInt("PlayerHealth:", component.Health);
	}

	private void SaveSoundTriggers()
	{
		LevelItemsManager.instance.RefreshSoundTriggersList();
		int count = LevelItemsManager.instance.soundTriggers.Count;
		for (int i = 0; i < count; i++)
		{
			PlayerPrefs.SetInt("SoundTrigger:" + i, LevelItemsManager.instance.soundTriggers[i].GetComponent<SoundTrigger>().id);
		}
		PlayerPrefs.SetInt("SoundTriggersCount", count);
	}

	private void SaveGameCameras()
	{
		int count = CameraManager.instance.gameCameras.Count;
		PlayerPrefs.SetInt("CamerasCount", count);
		for (int i = 0; i < count; i++)
		{
			PlayerPrefs.SetInt("Camera:" + i, CameraManager.instance.gameCameras[i].parent.GetComponent<RecordCamera_obj>().cameraID);
		}
	}

	private void SaveScreemers()
	{
		LevelItemsManager.instance.RefreshScreemersList();
		int count = LevelItemsManager.instance.screemers.Count;
		for (int i = 0; i < count; i++)
		{
			PlayerPrefs.SetInt("Screemer:" + i, LevelItemsManager.instance.screemers[i].GetComponent<ScreemerSystem>().id);
		}
		PlayerPrefs.SetInt("ScreemersCount", count);
	}

	private void SaveItems()
	{
		LevelItemsManager.instance.RefrashItemList();
		int count = LevelItemsManager.instance.items.Count;
		for (int i = 0; i < count; i++)
		{
			PlayerPrefs.SetInt("Item:" + i, LevelItemsManager.instance.items[i].GetComponent<Item>().id);
		}
		PlayerPrefs.SetInt("ItemsCount", count);
	}

	private void SaveTrapPosition()
	{
		LevelItemsManager.instance.RefreshTrapList();
		int count = LevelItemsManager.instance.trapObjects.Count;
		PlayerPrefs.SetInt("TrapsCount", count);
		for (int i = 0; i < count; i++)
		{
			Transform transform = LevelItemsManager.instance.trapObjects[i].transform;
			PlayerPrefs.SetFloat("Trap:" + i + " x=", transform.position.x);
			PlayerPrefs.SetFloat("Trap:" + i + " y=", transform.position.y);
			PlayerPrefs.SetFloat("Trap:" + i + " z=", transform.position.z);
		}
	}

	private void SavePlayerWeapon()
	{
		int count = WeaponManager.instance.playerWeapons.Count;
		PlayerPrefs.SetInt("WeaponsCount:", count);
		for (int i = 0; i < count; i++)
		{
			PlayerPrefs.SetInt("Weapon:" + i, WeaponManager.instance.playerWeapons[i].GetComponent<Weapon>().weaponID);
		}
	}

	private void SavePlayerAmmo()
	{
		int count = WeaponManager.instance.weaponsAmmo.Count;
		for (int i = 0; i < count; i++)
		{
			PlayerPrefs.SetInt("Ammo:" + i, WeaponManager.instance.weaponsAmmo[i]);
		}
	}

	public void LoadGame()
	{
		LoadWindows();
		LoadPlayerWeapon();
		LoadPlayerAmmo();
		LoadGameCameras();
		LoadLevelTraps();
		LoadItems();
		LoadScreemers();
		LoadSoundTriggers();
		LoadRakeHealth();
		LoadPlayerHealth();
		Debug.Log("Game Loaded");
	}

	private void LoadWindows()
	{
		int count = LevelItemsManager.instance.windowsObj.Count;
		for (int i = 0; i < count; i++)
		{
			if (PlayerPrefs.GetInt("Window:" + i) == 1)
			{
				LevelItemsManager.instance.windowsObj[i].GetComponent<Window_obj>().SetBrokenState();
			}
		}
		Debug.Log("Loaded glass");
	}

	private void LoadRakeHealth()
	{
		Enemy_AI component = GameObject.FindGameObjectWithTag("Rake").GetComponent<Enemy_AI>();
		component.Health = PlayerPrefs.GetFloat("RakeHealth:");
	}

	private void LoadPlayerHealth()
	{
		FPSController component = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
		component.Health = PlayerPrefs.GetInt("PlayerHealth:");
	}

	public void LoadPlayerWeapon()
	{
		WeaponManager.instance.playerWeapons.Clear();
		int @int = PlayerPrefs.GetInt("WeaponsCount:");
		for (int i = 0; i < @int; i++)
		{
			Transform item = WeaponManager.instance.allWeapons[PlayerPrefs.GetInt("Weapon:" + i)];
			WeaponManager.instance.playerWeapons.Add(item);
		}
	}

	public void LoadSoundTriggers()
	{
		int @int = PlayerPrefs.GetInt("SoundTriggersCount");
		LevelItemsManager.instance.RefreshSoundTriggersList();
		for (int i = 0; i < LevelItemsManager.instance.soundTriggers.Count; i++)
		{
			LevelItemsManager.instance.soundTriggers[i].SetActive(value: false);
		}
		for (int j = 0; j < @int; j++)
		{
			LevelItemsManager.instance.soundTriggers[PlayerPrefs.GetInt("SoundTrigger:" + j)].SetActive(value: true);
		}
	}

	public void LoadScreemers()
	{
		int @int = PlayerPrefs.GetInt("ScreemersCount");
		LevelItemsManager.instance.RefreshScreemersList();
		for (int i = 0; i < LevelItemsManager.instance.screemers.Count; i++)
		{
			LevelItemsManager.instance.screemers[i].SetActive(value: false);
		}
		for (int j = 0; j < @int; j++)
		{
			LevelItemsManager.instance.screemers[PlayerPrefs.GetInt("Screemer:" + j)].SetActive(value: true);
		}
	}

	public void LoadItems()
	{
		int @int = PlayerPrefs.GetInt("ItemsCount");
		LevelItemsManager.instance.RefrashItemList();
		for (int i = 0; i < LevelItemsManager.instance.items.Count; i++)
		{
			LevelItemsManager.instance.items[i].SetActive(value: false);
		}
		for (int j = 0; j < @int; j++)
		{
			LevelItemsManager.instance.items[PlayerPrefs.GetInt("Item:" + j)].SetActive(value: true);
		}
	}

	public void LoadLevelTraps()
	{
		LevelItemsManager.instance.trapObjects.Clear();
		int @int = PlayerPrefs.GetInt("TrapsCount");
		for (int i = 0; i < @int; i++)
		{
			Vector3 zero = Vector3.zero;
			zero.x = PlayerPrefs.GetFloat("Trap:" + i + " x=");
			zero.y = PlayerPrefs.GetFloat("Trap:" + i + " y=") + 1f;
			zero.z = PlayerPrefs.GetFloat("Trap:" + i + " z=");
			GameObject obj = (GameObject)Object.Instantiate(trapPrefab, zero, trapPrefab.transform.rotation);
			LevelItemsManager.instance.AddTrap(obj);
		}
	}

	public void LoadGameCameras()
	{
		CameraManager.instance.gameCameras.Clear();
		int @int = PlayerPrefs.GetInt("CamerasCount");
		for (int i = 0; i < @int; i++)
		{
			if (CameraManager.instance.allCameras[PlayerPrefs.GetInt("Camera:" + i)] != null)
			{
				CameraManager.instance.allCameras[PlayerPrefs.GetInt("Camera:" + i)].GetComponent<RecordCamera_obj>().SetSetupState();
			}
		}
	}

	public void LoadPlayerAmmo()
	{
		int count = WeaponManager.instance.weaponsAmmo.Count;
		for (int i = 0; i < count; i++)
		{
			WeaponManager.instance.weaponsAmmo[i] = PlayerPrefs.GetInt("Ammo:" + i);
		}
	}
}
