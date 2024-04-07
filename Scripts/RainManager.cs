// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// RainManager
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
	public List<GameObject> rains = new List<GameObject>();

	public Transform rain;

	public Transform currentPlayer;

	public Transform defaultPlayer;

	public static RainManager instance;

	private void Start()
	{
		instance = this;
	}

	public void SetPlayerRainState(bool state)
	{
		rains[0].SetActive(state);
	}

	public void Update()
	{
		if (currentPlayer != null)
		{
			Vector3 position = currentPlayer.position;
			position.y += 10f;
			rain.position = position;
		}
	}
}
