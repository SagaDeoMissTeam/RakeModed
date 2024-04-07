// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AnimalCage_obj
using UnityEngine;

public class AnimalCage_obj : MonoBehaviour
{
	public Animator anim;

	private bool bTraped;

	public GameObject currentAnimal;

	private void OnTriggerEnter(Collider info)
	{
		if ((bool)info.GetComponent<Animal_AI>())
		{
			currentAnimal = info.gameObject;
			CloseCage();
		}
	}

	public void OpenCage()
	{
		anim.SetBool("Close", value: false);
		if (currentAnimal != null)
		{
			currentAnimal.GetComponent<Animal_AI>().AfterTrapEffect();
		}
	}

	public void CloseCage()
	{
		anim.SetBool("Close", value: true);
		if (currentAnimal != null)
		{
			currentAnimal.GetComponent<Animal_AI>().Event_Trapped();
		}
	}
}
