// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_AnimalDetectionTrigger
using UnityEngine;

public class MP_AnimalDetectionTrigger : MonoBehaviour
{
	private MP_Animal bot;

	public void OnTriggerEnter(Collider info)
	{
		bot.OnObjectEnter(info);
	}

	private void Awake()
	{
		bot = GetComponentInParent<MP_Animal>();
	}

	private void Update()
	{
	}
}
