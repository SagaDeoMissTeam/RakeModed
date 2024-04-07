// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_PlayerSpawnManager
using UnityEngine;

public class MP_PlayerSpawnManager : MonoBehaviour
{
	public GameObject PlayerPrefab;

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 20f, 100f, 60f), "Spawn player"))
		{
			SpawnPlayer();
		}
	}

	private void SpawnPlayer()
	{
		Network.Instantiate(PlayerPrefab, base.transform.position, Quaternion.identity, 0);
	}
}
