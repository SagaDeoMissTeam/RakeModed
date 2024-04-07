// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Trap_proj
using UnityEngine;

public class Trap_proj : MonoBehaviour
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
			component.GetComponent<FPSController>().TakeDamage(35, "TrapDamage");
		}
		if (info.GetComponent<Collider>().tag == "Rake" && bSetup)
		{
			Collider component2 = info.GetComponent<Collider>();
			GetComponent<AudioSource>().PlayOneShot(activeSound);
			SetTrapActive();
			component2.GetComponent<Enemy_AI>().TakeDamage(35, "TrapDamage");
		}
		if ((bool)info.GetComponent<Animal_AI>() && bSetup)
		{
			Collider component3 = info.GetComponent<Collider>();
			GetComponent<AudioSource>().PlayOneShot(activeSound);
			SetTrapActive();
			component3.GetComponent<Animal_AI>().TakeDamage(100, "TrapDamage");
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
