// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AnimalsManager
using System.Collections.Generic;
using UnityEngine;

public class AnimalsManager : MonoBehaviour
{
	public List<Transform> currentAnimals = new List<Transform>();

	public List<Transform> spawnPoints = new List<Transform>();

	public List<GameObject> animalsType = new List<GameObject>();

	public List<Transform> foodPoints = new List<Transform>();

	public int prevPointID;

	public static AnimalsManager instance;

	private void Start()
	{
		instance = this;
	}

	public void FindClosetAnimal(Transform point)
	{
		RefreshList();
		int index = 0;
		if (point == null)
		{
			return;
		}
		float num = 0f;
		if (currentAnimals[0] != null)
		{
			num = Vector3.Distance(point.position, currentAnimals[0].position);
		}
		for (int i = 0; i < currentAnimals.Count; i++)
		{
			if (Vector3.Distance(point.position, currentAnimals[i].position) < num)
			{
				num = Vector3.Distance(point.position, currentAnimals[i].position);
				index = i;
			}
		}
		Debug.Log(num);
		if (num < 250f)
		{
			currentAnimals[index].GetComponent<Animal_AI>().foodPoints.Add(point);
		}
	}

	public void RefreshList()
	{
		for (int i = 0; i < currentAnimals.Count; i++)
		{
			if (currentAnimals[i] == null)
			{
				currentAnimals.RemoveAt(i);
			}
		}
	}

	public void SpawnAnimal()
	{
		int index = Random.Range(0, spawnPoints.Count);
		int index2 = Random.Range(0, animalsType.Count);
		Object.Instantiate(animalsType[index2], spawnPoints[index].position, Quaternion.identity);
	}

	public List<Transform> GetFoodPoints()
	{
		return foodPoints;
	}

	public void AddFoodPoint(Transform point)
	{
		foodPoints.Add(point);
		FindClosetAnimal(point);
	}

	public void RemoveFoodPoint(Transform point)
	{
		foodPoints.Remove(point);
	}
}
