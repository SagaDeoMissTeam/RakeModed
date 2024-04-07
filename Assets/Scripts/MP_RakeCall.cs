// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_RakeCall
using UnityEngine;

public class MP_RakeCall : MonoBehaviour
{
	private void OnTriggerEnter(Collider info)
	{
		if (info.tag == "Player")
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Rake");
			gameObject.GetComponent<MP_Enemy>().runPoint = base.transform.position;
			gameObject.GetComponent<MP_Enemy>().ChangeState(MP_Enemy.ENEMY_STATE.RUN);
			Network.Destroy(base.gameObject);
		}
	}
}
