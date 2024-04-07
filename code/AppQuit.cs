// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AppQuit
using UnityEngine;

public class AppQuit : MonoBehaviour, IQuittable
{
	public void OnQuit()
	{
		Debug.Log("AppQuit.Quit");
		Application.Quit();
	}
}
