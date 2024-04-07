// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_EnemyDetectionTrigger
using UnityEngine;

public class MP_EnemyDetectionTrigger : MonoBehaviour
{
	private MP_Enemy bot;

	public void OnTriggerEnter(Collider info)
	{
		bot.OnObjectEnter(info);
	}

	public void OnTriggerStay(Collider info)
	{
		bot.OnObjectStay(info);
	}

	private void Awake()
	{
		bot = GetComponentInParent<MP_Enemy>();
	}

	private void Update()
	{
	}
}
