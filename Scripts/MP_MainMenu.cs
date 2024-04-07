// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_MainMenu
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MP_MainMenu : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> windows = new List<GameObject>();

	[SerializeField]
	private GameObject currentMenu;

	private MP_NetworkManager networkManager;

	private MP_ErrorMassage errorMassage;

	[SerializeField]
	private string serverName;

	[SerializeField]
	private string connectIP;

	[SerializeField]
	private string connectPort;

	[SerializeField]
	private string ownerIP;

	[SerializeField]
	private string port;

	[SerializeField]
	private List<Text> playerList = new List<Text>();

	private void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		networkManager = GetComponent<MP_NetworkManager>();
		errorMassage = GetComponent<MP_ErrorMassage>();
	}

	public void OpenMenu(int id)
	{
		windows[id].SetActive(value: true);
		currentMenu.SetActive(value: false);
		currentMenu = windows[id];
	}

	public void LoadingLevel()
	{
		OpenMenu(6);
	}

	private void OnServerInitialized()
	{
		OpenMenu(2);
		ownerIP = GetComponent<NetworkView>().owner.ipAddress;
		port = string.Empty + GetComponent<NetworkView>().owner.port;
		RefreshPlayerList();
	}

	private void OnConnectedToServer()
	{
		RefreshPlayerList();
		OpenMenu(4);
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		errorMassage.OpenErrorWindow(info.ToString());
		OpenMenu(0);
	}

	private void OnFailedToConnect(NetworkConnectionError info)
	{
		errorMassage.OpenErrorWindow(info.ToString());
	}

	public void SetServerName(string input)
	{
		serverName = input;
	}

	public void SetConnectIP(string input)
	{
		connectIP = input;
	}

	public void SetConnectPort(string input)
	{
		connectPort = input;
	}

	public void RefreshPlayerList()
	{
		for (int i = 0; i < playerList.Count; i++)
		{
			playerList[i].text = string.Empty;
		}
		for (int j = 0; j < networkManager.PlayerList.Count; j++)
		{
			playerList[j].text = networkManager.PlayerList[j].PlayerName;
		}
	}

	public void ShutdownServer()
	{
		Network.Disconnect();
	}

	public void Disconnect()
	{
		Network.Disconnect();
	}

	public void StartServer()
	{
		if (serverName == string.Empty)
		{
			serverName = "RakeGameID" + Random.Range(0f, float.PositiveInfinity);
		}
		networkManager.StartServer(serverName, string.Empty, 4, "RAKE", 2500);
	}

	public void StartGame()
	{
		networkManager.GetComponent<NetworkView>().RPC("LoadMap", RPCMode.All, 1);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void ConnectToServer()
	{
		if (connectIP == string.Empty)
		{
			errorMassage.OpenErrorWindow("Please enter server IP");
		}
		else if (connectPort == string.Empty)
		{
			errorMassage.OpenErrorWindow("Please enter server port");
		}
		else
		{
			Network.Connect(connectIP, int.Parse(connectPort));
		}
	}
}
