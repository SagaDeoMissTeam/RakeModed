// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// TimePause
using UnityEngine;

public class TimePause : MonoBehaviour, IPausable
{
	public float pauseDelay = 0.3f;

	private float timeScale;

	public void OnUnPause()
	{
		Debug.Log("TestPause.OnUnPause");
		Time.timeScale = timeScale;
	}

	public void OnPause()
	{
		Debug.Log("TestPause.OnPause");
		timeScale = Time.timeScale;
		Invoke("StopTime", pauseDelay);
	}

	private void StopTime()
	{
		Time.timeScale = 0f;
	}
}
