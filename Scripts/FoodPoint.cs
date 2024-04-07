// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// FoodPoint
using UnityEngine;

public class FoodPoint : MonoBehaviour
{
	public int count;

	public bool isUse;

	public void Eat()
	{
		if (count == -1)
		{
			return;
		}
		count--;
		if (count <= 0)
		{
			AnimalsManager.instance.RemoveFoodPoint(base.transform);
			if (base.transform.parent != null)
			{
				Object.Destroy(base.transform.parent.gameObject);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
