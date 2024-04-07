// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Menu_Settings
using System;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Settings : MonoBehaviour
{
	[SerializeField]
	private string playerName;

	[SerializeField]
	private InputField inputField;

	private void OnEnable()
	{
		if (!inputField)
		{
			inputField = GetComponentInChildren<InputField>();
		}
		string text = ((!(PlayerPrefs.GetString("PlayerName") == string.Empty)) ? PlayerPrefs.GetString("PlayerName") : Environment.UserName);
		playerName = text;
		inputField.text = playerName;
	}

	public void SetPlayerName(string param)
	{
		playerName = param;
	}

	public void SaveName()
	{
		PlayerPrefs.SetString("PlayerName", playerName);
	}
}
