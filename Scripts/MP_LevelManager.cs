// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_LevelManager
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MP_LevelManager : MonoBehaviour
{
	public static MP_LevelManager instance;

	public GameObject text;

	private void Awake()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			instance = this;
		}
	}

	public void FinishGame()
	{
		GetComponent<NetworkView>().RPC("ShowGUI", RPCMode.All);
	}

	[RPC]
	public void ShowGUI()
	{
		text.SetActive(value: true);
		text.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName") + " WIN!!!";
		StartCoroutine(finishGame());
	}

	private IEnumerator finishGame()
	{
		yield return new WaitForSeconds(6f);
		Network.Disconnect();
		Application.LoadLevel(0);
	}

	public void Exit()
	{
		Network.Disconnect();
		Application.LoadLevel(0);
	}
}
