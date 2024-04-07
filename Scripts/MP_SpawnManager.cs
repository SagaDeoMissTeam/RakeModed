// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_SpawnManager
using UnityEngine;

public class MP_SpawnManager : MonoBehaviour
{
	[SerializeField]
	private GameObject spawnCanvas;

	public GameObject rakePrefab;

	public GameObject hunterPrefab;

	public GameObject botPrefab;

	public GameObject PlayerCanvas;

	public Transform rakeSpawn;

	public Transform playerSpawnPos;

	private void Start()
	{
		SpawnBot();
		SpawnHunterPlayer();
	}

	private void OnLevelWasLoaded()
	{
	}

	public void OnEnable()
	{
		spawnCanvas.SetActive(value: true);
	}

	[RPC]
	public void st()
	{
		spawnCanvas.SetActive(value: true);
	}

	public void SpawnRakePlayer()
	{
		GameObject gameObject = Network.Instantiate(rakePrefab, base.transform.position, Quaternion.identity, 0) as GameObject;
		gameObject.GetComponent<MP_RPSController>().spawnManager = base.gameObject;
		base.gameObject.SetActive(value: false);
		spawnCanvas.SetActive(value: false);
	}

	public void SpawnHunterPlayer()
	{
		GameObject gameObject = Network.Instantiate(hunterPrefab, playerSpawnPos.position, Quaternion.identity, 0) as GameObject;
		gameObject.GetComponent<MP_FPSController>().spawnManager = base.gameObject;
		base.gameObject.SetActive(value: false);
		spawnCanvas.SetActive(value: false);
	}

	public void SpawnBot()
	{
		if (Network.isServer)
		{
			Network.Instantiate(botPrefab, rakeSpawn.position, Quaternion.identity, 0);
		}
	}
}
