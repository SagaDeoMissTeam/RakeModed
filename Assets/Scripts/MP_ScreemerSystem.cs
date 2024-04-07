// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_ScreemerSystem
using System.Collections;
using UnityEngine;

public class MP_ScreemerSystem : MonoBehaviour
{
	public bool action;

	public GameObject rake;

	public string DmgType;

	public int id;

	public AudioClip triggerSound;

	public AudioClip attackSound;

	public float timeOnAttack;

	public float timeDelay;

	public bool bTakeDamage = true;

	private GameObject playerObj;

	public Transform playerViewPoint;

	private bool bProcess;

	private void Start()
	{
	}

	private void Update()
	{
		if (bProcess)
		{
			playerObj.GetComponent<MP_CameraMouseLook>().Focus(playerViewPoint.position);
		}
	}

	private void OnTriggerEnter(Collider player)
	{
		if (player.tag == "Player" && !action)
		{
			AudioSource.PlayClipAtPoint(triggerSound, base.transform.position);
			playerObj = player.gameObject;
			StartCoroutine(playerDamage());
			GetComponent<NetworkView>().RPC("ActiveScreemer", RPCMode.All);
		}
	}

	[RPC]
	public void ActiveScreemer()
	{
		AudioSource.PlayClipAtPoint(triggerSound, base.transform.position);
		action = true;
	}

	private IEnumerator playerDamage()
	{
		yield return new WaitForSeconds(timeDelay);
		AudioSource.PlayClipAtPoint(triggerSound, base.transform.position);
		playerObj.GetComponent<MP_CameraMouseLook>().Focus(playerViewPoint.position);
		playerObj.GetComponent<NetworkView>().RPC("FreezePlayer", RPCMode.All, false);
		playerObj.GetComponent<MP_CameraMouseLook>().bControl = false;
		rake.SetActive(value: true);
		bProcess = true;
		rake.GetComponent<Animation>().Play();
		yield return new WaitForSeconds(timeOnAttack);
		bProcess = false;
		AudioSource.PlayClipAtPoint(attackSound, base.transform.position);
		if (bTakeDamage)
		{
			playerObj.GetComponent<NetworkView>().RPC("TakeDamage", RPCMode.All, 10, "Kick");
		}
		playerObj.GetComponent<NetworkView>().RPC("FreezePlayer", RPCMode.All, true);
		playerObj.GetComponent<MP_CameraMouseLook>().bControl = true;
		yield return new WaitForSeconds(0.5f);
		Object.Destroy(rake);
		Network.Destroy(base.gameObject);
	}
}
