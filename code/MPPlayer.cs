// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MPPlayer
using System;
using UnityEngine;

[Serializable]
public class MPPlayer
{
	public string PlayerName = string.Empty;

	public NetworkPlayer PlayerNet;

	public bool bReady;
}
