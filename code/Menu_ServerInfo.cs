// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Menu_ServerInfo
using UnityEngine;
using UnityEngine.UI;

public class Menu_ServerInfo : MonoBehaviour
{
	[SerializeField]
	private Text players;

	[SerializeField]
	private Text ipAdress;

	[SerializeField]
	private Text port;

	[SerializeField]
	private Text peerType;

	[SerializeField]
	private MP_NetworkManager netManager;

	private void Start()
	{
	}

	private void Update()
	{
		players.text = "Players: " + netManager.PlayerList.Count;
		ipAdress.text = "IP adress: " + netManager.GetComponent<NetworkView>().owner.ipAddress;
		port.text = "Port: " + netManager.GetComponent<NetworkView>().owner.port;
		peerType.text = "PeerType: " + Network.peerType;
	}
}
