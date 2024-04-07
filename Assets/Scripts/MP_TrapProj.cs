// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_TrapProj
using UnityEngine;

public class MP_TrapProj : MonoBehaviour
{
	public bool bSetup;

	public GameObject trap;

	public AudioClip activeSound;

	public AudioClip setupSound;

	public GameObject particle;

	private void OnTriggerEnter(Collider info)
	{
		if (info.GetComponent<Collider>().tag == "Player" && bSetup)
		{
			Collider component = info.GetComponent<Collider>();
			GetComponent<AudioSource>().PlayOneShot(activeSound);
			SetTrapActive();
			component.GetComponent<NetworkView>().RPC("TakeDamage", RPCMode.All, 35, "Kick");
		}
		if (info.GetComponent<Collider>().tag == "Rake" && bSetup)
		{
			Collider component2 = info.GetComponent<Collider>();
			GetComponent<AudioSource>().PlayOneShot(activeSound);
			SetTrapActive();
			component2.GetComponent<NetworkView>().RPC("TakeDamage", RPCMode.All, 35);
		}
	}

	public void SetTrapActive()
	{
		trap.GetComponent<Animation>().Play();
		Object.Instantiate(particle, base.transform.position, Quaternion.identity);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if ((bool)collision.collider.GetComponent<TerrainCollider>())
		{
			Ray ray = new Ray(base.transform.position, Vector3.down);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 0.4f))
			{
				bSetup = true;
				GetComponent<AudioSource>().PlayOneShot(setupSound);
				base.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
			}
		}
	}
}
