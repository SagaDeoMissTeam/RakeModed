// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_NetworkManager
using System;
using System.Collections.Generic;
using UnityEngine;

public class MP_NetworkManager : MonoBehaviour
{
	public List<MPPlayer> PlayerList = new List<MPPlayer>();

	public List<MapSettings> MapList = new List<MapSettings>();

	public List<string> MassageList = new List<string>();

	public string playerNameMain;

	public MapSettings currentMap;

	public int maxPlayers;

	private MP_MainMenu menu;

	private void Start()
	{
		menu = GetComponent<MP_MainMenu>();
		currentMap = MapList[0];
		string text = ((!(PlayerPrefs.GetString("PlayerName") == string.Empty)) ? PlayerPrefs.GetString("PlayerName") : Environment.UserName);
		playerNameMain = text;
	}

	public void StartServer(string serverName, string serverPassword, int maxPlayers, string description, int port)
	{
		Network.InitializeSecurity();
		if (serverPassword != string.Empty)
		{
			Network.incomingPassword = serverPassword;
		}
		Network.InitializeServer(maxPlayers, port, useNat: true);
		MasterServer.RegisterHost("RAKE(ID386880)", serverName, description);
	}

	private void OnDisconnectedFromServer()
	{
		PlayerList.Clear();
		menu.RefreshPlayerList();
	}

	private void OnPlayerDisconnected(NetworkPlayer id)
	{
		GetComponent<NetworkView>().RPC("RemovePlayer", RPCMode.All, id);
		menu.RefreshPlayerList();
	}

	private void OnConnectedToServer()
	{
		playerNameMain = PlayerPrefs.GetString("PlayerName");
		GetComponent<NetworkView>().RPC("PlayerJoinRequest", RPCMode.Server, playerNameMain, Network.player);
	}

	private void OnPlayerConnected(NetworkPlayer player)
	{
		foreach (MPPlayer player2 in PlayerList)
		{
			GetComponent<NetworkView>().RPC("AddPlayerToList", player, player2.PlayerName, player2.PlayerNet);
		}
	}

	private void OnServerInitialized()
	{
		PlayerJoinRequest(playerNameMain, Network.player);
	}

	[RPC]
	private void KickPlayer(NetworkPlayer pl)
	{
		Network.CloseConnection(pl, sendDisconnectionNotification: true);
	}

	[RPC]
	private void SetPlayerStatus(NetworkPlayer player, bool status)
	{
		foreach (MPPlayer player2 in PlayerList)
		{
			if (player2.PlayerNet == player)
			{
				player2.bReady = status;
			}
		}
	}

	[RPC]
	private void PlayerJoinRequest(string playerName, NetworkPlayer views)
	{
		GetComponent<NetworkView>().RPC("AddPlayerToList", RPCMode.All, playerName, views);
	}

	[RPC]
	private void AddPlayerToList(string playerName, NetworkPlayer view)
	{
		MP_ConnectionsCounter.DefaultInstance.connections++;
		MPPlayer mPPlayer = new MPPlayer();
		mPPlayer.PlayerName = playerName;
		mPPlayer.PlayerNet = view;
		PlayerList.Add(mPPlayer);
		menu.RefreshPlayerList();
	}

	[RPC]
	private void LoadMap(int prefix)
	{
		MP_ConnectionsCounter.DefaultInstance.GetComponent<NetworkView>().RPC("SetConnection", RPCMode.All, PlayerList.Count);
		menu.LoadingLevel();
		Network.SetLevelPrefix(prefix);
		Application.LoadLevel(currentMap.MapLoadName);
	}

	[RPC]
	private void RemovePlayer(NetworkPlayer view)
	{
		MPPlayer mPPlayer = new MPPlayer();
		foreach (MPPlayer player in PlayerList)
		{
			if (player.PlayerNet == view)
			{
				mPPlayer = player;
			}
		}
		if (mPPlayer != null)
		{
			PlayerList.Remove(mPPlayer);
		}
	}
}
