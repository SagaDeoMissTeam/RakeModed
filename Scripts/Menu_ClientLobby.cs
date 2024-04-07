// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Menu_ClientLobby
using UnityEngine;
using UnityEngine.UI;

public class Menu_ClientLobby : MonoBehaviour
{
	[SerializeField]
	private Text playersConnected;

	[SerializeField]
	private MP_NetworkManager netManager;

	private void Start()
	{
	}

	private void Update()
	{
		playersConnected.text = "Players: " + netManager.PlayerList.Count + "/4";
	}
}
